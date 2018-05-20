using System.IO;
using Microsoft.AspNetCore.Mvc;

#region SampleClientController
public class SampleClientController : ControllerBase
{
    [HttpGet]
    [Route("SampleClient")]
    public IActionResult SampleClient()
    {
        var file = Path.Combine(Directory.GetCurrentDirectory(), "SampleClient.html");
        return PhysicalFile(file, "text/html");
    }
}

#endregion