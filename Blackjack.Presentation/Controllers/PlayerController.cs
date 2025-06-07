using Blackjack.Business.Services.Interfaces;
using Blackjack.Presentation.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
    
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
    
    [HttpPost("authenticate")]
    public IActionResult Auth([FromBody] PlayerAuthRequest request)
    {
        
        return Ok();
    }
}