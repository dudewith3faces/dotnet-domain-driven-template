using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("")]
public class MonitorController : ApiControllerBase
{
    [HttpGet]
    public IActionResult Server() => CreateResponse();
}