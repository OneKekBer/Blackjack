using Blackjack.GameLogic.Handlers;
using Blackjack.Shared.Interfaces;
using Blackjack.Shared.Models;
using Blackjack.Shared.Types;

namespace Blackjack.GameLogic;

public class GameEngine
{
    private readonly IInputService _inputService;
    private readonly IOutputService _outputService;
    private readonly DeckHandler _deckHandler = new();
    private readonly int _numberOfPlayers = 0;
    
    public GameEngine(IInputService inputService, IOutputService outputService)
    {
        _inputService = inputService;
        _outputService = outputService;
    }

    public void Turn()
    {
        
    }

    public void Start(List<Player> players)
    {
        _deckHandler.ResetDeck();
        var playerHand = new Hand();
        
        playerHand.AddCard(_deckHandler.GetCard());
        _outputService.ShowPlayerHand(playerHand.Cards, playerHand.GetScore());
 

        while (true)
        {
            var action = _inputService.GetPlayerAction();

            if (action != PlayerAction.Hit) break;
            playerHand.AddCard(_deckHandler.GetCard());
            _outputService.ShowPlayerHand(playerHand.Cards, playerHand.GetScore());
        }
    }

}

