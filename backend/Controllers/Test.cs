using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("main")]
    public string Getmain()
    {
        return "welcome";
    }
}
