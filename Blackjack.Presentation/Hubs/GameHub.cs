

using Blackjack.Presentation.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Blackjack.Presentation.Hubs;


// get: back <- front
// send: back -> front

public class GameHub : Hub<IGameHubClient>, IGameHub
{
    
}