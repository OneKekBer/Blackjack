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
    public IActionResult Create()
    {
        _gameService.Create();
        return Ok();
    }
    
    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
        var games = _gameService.GetAll();
        _logger.LogInformation($"Returning {games.Result.Count()}");
        return Ok(games);
    }
}