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
    
    public GameEngine(IInputService inputService, IOutputService outputService, IGamePersisterService gamePersisterService)
    {
        _inputService = inputService;
        _outputService = outputService;
        _gamePersisterService = gamePersisterService;
    }
    
    public void InitGame(Game game)
    {
        game.Deck = DeckHandler.NewDeck();
        game.Status = GameStatus.Started;
    
        game.TurnQueue = new Queue<Guid>(
            game.Players
                .Where(p => p.IsPlaying)
                .Select(p => p.Id)
        );
    }
    
    private async Task<PlayerAction> GetPlayerAction(Player player, Guid gameId)
    {
        return player.Role == Role.Bot
            ? _botHandler.GetAction(player.Cards.GetScore())
            : await _inputService.GetPlayerAction(gameId, player.Id);
    }
    
    private void HandlePlayerAction(Player player, PlayerAction action, Game game)
    {
        if (action == PlayerAction.Stand)
        {
            player.IsPlaying = false;
            
            return;
        }
        game.TurnQueue.Enqueue(player.Id);
        player.Cards.Add(GameHandler.GetCard(game.Deck));

        if (player.Role == Role.Bot) return;
        
        _outputService.ShowPlayerHand(
            game.Id,
            player.Id,
            player.Cards,
            player.Cards.GetScore());
    }
    
    private async Task PlayRound(Game game)
    {
        while (game.TurnQueue.Any())
        {
            var currentPlayerId = game.TurnQueue.First();
            var currentPlayer = game.Players.SingleOrDefault(p => p.Id == currentPlayerId);
                
            if (currentPlayer == null || !currentPlayer.IsPlaying)
                continue;
            
            await _outputService.SendNewTurnPlayerId(game.Id, game.Players, currentPlayerId);
            
            var action = await GetPlayerAction(currentPlayer, game.Id);
            game.TurnQueue.Dequeue();
            await _gamePersisterService.SaveGame(game);
            HandlePlayerAction(currentPlayer, action, game);

            await _gamePersisterService.SaveGame(game);
            await _outputService.SendGameState(game);
        }
    }
    
    public async Task Start(Guid gameId)
    {
        while (true) // maybe change on while CancellationToken != None
        {
            var game = await _gamePersisterService.LoadGame(gameId);
            //validate game on only bots/one player/mb smth else
            GameHandler.CheckPlayers(game);
            //if (_game.Players.Count == 1)
            //    break; // create handling mb output.BreakGame
            
            await PlayRound(game);
        
            var winnersIds = GameHandler.GetWinnersId(game);
            var winnersName = game.Players
                .Where(p => winnersIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToList();
        
            GameHandler.GivePrizes(game, winnersIds);

            var message = MessagesGenerator.GenerateResultMessage(winnersName);
            await _outputService.SendResult(game.Id, message, game.Players);
            //checkOnNewPlayersMethod?  
            GameHandler.ResetGame(game);
            
            await _gamePersisterService.SaveGame(game);
            await _outputService.SendGameState(game);
        }
    }
}

