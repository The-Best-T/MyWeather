using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LocationsController : ControllerBase
{

    [HttpGet]
    public ActionResult<string> GetTest()
    {
        return Ok("1290210190");
    }
}