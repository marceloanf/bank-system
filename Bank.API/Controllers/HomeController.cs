using Microsoft.AspNetCore.Mvc;

namespace Bank.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Bank API");
    }
}
