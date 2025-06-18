using Blackjack.Business.Services.Interfaces;
using Blackjack.Presentation.Contracts.Requests;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blackjack.Presentation.Controllers;

[ApiController()]
[Route("api/player")]
[EnableCors("AllowAllOrigins")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
        
    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    
    [HttpPost("change-name")]
    public async Task<IActionResult> ChangeName([FromBody] ChangePlayerNameRequest request)
    {
        await _playerService.ChangePlayerName(request.PlayerId, request.PlayerId, request.UserId, request.NewName);
        
        return Ok();
    }
}