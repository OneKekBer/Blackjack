using Microsoft.AspNetCore.Mvc;

namespace Blackjack.Presentation.Controllers;

public class LobbyController : ControllerBase
{
    public IActionResult GetAll()
    {
        return Ok();
    }
}