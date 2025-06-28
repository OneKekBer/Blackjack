using Blackjack.GameLogic.Extensions;
using Blackjack.GameLogic.Handlers;
using Blackjack.GameLogic.Helpers;
using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic;

public class GameEngine 
{
    private readonly IInputService _inputService;
    private readonly IGamePersisterService _gamePersisterService;
    private readonly IOutputService _outputService;
    private readonly BotHandler _botHandler = new();
    private Game _game;
    
    public GameEngine(IInputService inputService, IOutputService outputService, IGamePersisterService gamePersisterService)
    {
        _inputService = inputService;
        _outputService = outputService;
        _gamePersisterService = gamePersisterService;
    }
    
    public void InitGame(Game game)
    {
        _game = game;
        _game.Deck = DeckHandler.NewDeck();
        _game.Status = GameStatus.Started;
    
        _game.TurnQueue = new Queue<Guid>(
            _game.Players
                .Where(p => p.IsPlaying)
                .Select(p => p.Id)
        );
    }
    
    private async Task<PlayerAction> GetPlayerAction(Player player)
    {
        return player.Role == Role.Bot
            ? _botHandler.GetAction(player.Cards.GetScore())
            : await _inputService.GetPlayerAction(_game.Id, player.Id);
    }
    
    private void HandlePlayerAction(Player player, PlayerAction action)
    {
        if (action == PlayerAction.Stand)
        {
            player.IsPlaying = false;
            
            return;
        }
        _game.TurnQueue.Enqueue(player.Id);
        player.Cards.Add(GameHandler.GetCard(_game));

        if (player.Role == Role.Bot) return;
        
        _gamePersisterService.SaveGame(_game);
        _outputService.ShowPlayerHand(
            _game.Id,
            player.Id,
            player.Cards,
            player.Cards.GetScore());
    }
    
    private async Task PlayRound()
    {
        while (_game.TurnQueue.Any())
        {
            var currentPlayerId = _game.TurnQueue.First();
            var currentPlayer = _game.Players.SingleOrDefault(p => p.Id == currentPlayerId);
                
            if (currentPlayer == null || !currentPlayer.IsPlaying)
                continue;
            
            await _outputService.SendNewTurnPlayerId(_game.Id, currentPlayerId);
            
            var action = await GetPlayerAction(currentPlayer);
            _game.TurnQueue.Dequeue();
            await _gamePersisterService.SaveGame(_game);
            HandlePlayerAction(currentPlayer, action);
            
            await _gamePersisterService.SaveGame(_game); // i don`t sure how correctly this method SAVE game
            await _outputService.SendGameStateById(_game.Id);
        }
    }
    
    public async Task Start()
    {
        while (true)
        {
            //validate game on only bots/one player/mb smth else
            GameHandler.CheckPlayers(_game);
            //if (_game.Players.Count == 1)
            //    break; // create handling mb output.BreakGame
            
            await PlayRound();
        
            var winnersIds = GameHandler.GetWinnersId(_game);
            var winnersName = _game.Players
                .Where(p => winnersIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToList();
        
            GameHandler.GivePrizes(_game, winnersIds);

            var message = MessagesGenerator.GenerateResultMessage(winnersName);
            await _outputService.SendResult(_game.Id, message, _game.Players);
            
            GameHandler.ResetGame(_game);
            
            await _gamePersisterService.SaveGame(_game);
            await _outputService.SendGameState(_game);
        }
    }
}

