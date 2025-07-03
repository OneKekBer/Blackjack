using Blackjack.Business.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blackjack.Presentation.Controllers;

[ApiController()]
[Route("api/game")]
[EnableCors("AllowAllOrigins")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;
    
    public GameController(IGameService gameService, ILogger<GameController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await _gameService.Create(cancellationToken);
        return Ok();
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var games = await _gameService.GetAll(cancellationToken);
        _logger.LogInformation($"Returning {games.Count()}");
        return Ok(games);
    }
    
    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken)
    {
        await _gameService.DeleteAll(cancellationToken);
        return Ok();
    }
}