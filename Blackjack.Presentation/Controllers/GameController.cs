using Blackjack.GameLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blackjack.Presentation.Controllers;

public class GameController : ControllerBase
{
    [HttpGet]
    public IActionResult Play()
    {
        
        return Ok();
    }
}