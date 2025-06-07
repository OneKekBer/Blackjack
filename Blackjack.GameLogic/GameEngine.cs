using Blackjack.GameLogic.Extensions;
using Blackjack.GameLogic.Handlers;
using Blackjack.GameLogic.Interfaces;
using Blackjack.GameLogic.Models;
using Blackjack.GameLogic.Other.Exception;
using Blackjack.GameLogic.Types;

namespace Blackjack.GameLogic;

public class GameEngine : IGameEngine
{
    private readonly IInputService _inputService;
    private readonly IOutputService _outputService;
    private readonly BotHandler _botHandler = new();
    private Game _game;
    
    public GameEngine(IInputService inputService, IOutputService outputService)
    {
        _inputService = inputService;
        _outputService = outputService;
    }
    
    public void InitGame(Game game)
    {
        _game = game;
        _game.CurrentPlayerIndex = 0;
        _game.Deck = DeckHandler.NewDeck();
        _game.Status = GameStatus.Started;
    }
    
    private void PlayRound()
    {
        var players = _game.Players ?? throw new NotInitializedPlayersException("Players in this game not yet initialized.");

        for (int i = 0; i < players.Count; i++)
        {
            _game.CurrentPlayerIndex = i;
            var currentPlayer = players[i];
            
            if (currentPlayer.IsPlaying == false)
                continue;
            
            var action = currentPlayer.Role == Role.Bot
                ? _botHandler.Logic(currentPlayer.Cards.GetScore())
                : _inputService.GetPlayerAction(_game.Id,currentPlayer.Id).Result;

            if (action is PlayerAction.Stand)
            {
                currentPlayer.IsPlaying = false;
                continue;
            }
            
            currentPlayer.Cards.Add(GameHandler.GetCard(_game));
            
            if (currentPlayer.Role != Role.Bot) 
                _outputService.ShowPlayerHand(
                    _game.Id,
                    currentPlayer.Id,
                    currentPlayer.Cards,
                    currentPlayer.Cards.GetScore());
        }
    }
    
    public void Start()
    {
        while (true)
        {
            GameHandler.CheckPlayers(_game);
            if (_game.Players.Count == 1)
                break;
            
            while (GameHandler.IsGameContinue(_game.Players))
                PlayRound();
        
            var winnersIds = GameHandler.GetWinnersId(_game);
            var winnersName = _game.Players
                .Where(p => winnersIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToList();
        
            GameHandler.GivePrizes(_game, winnersIds);

            var message = GenerateResultMessage(winnersName);
            _outputService.ShowResult(_game.Id, message, _game.Players);
            GameHandler.ResetGame(_game);
        }
    }

    private string GenerateResultMessage(List<string> winnersName) // remove in other file
    {
        return winnersName.Count switch
        {
            0 => "Nobody won the game.",
            1 => $"{winnersName[0]} won the game.",
            _ => $"{string.Join(", ", winnersName)} won the game."
        };
    }
}

