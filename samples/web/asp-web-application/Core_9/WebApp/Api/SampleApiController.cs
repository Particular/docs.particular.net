using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api;

[ApiController]
[Route("api/[controller]")]

#region MessageSessionInjectionWebApi
public class SampleApiController(IMessageSession messageSession) : ControllerBase
#endregion
{
    #region WebApiSendMessage
    [HttpGet]
    public async Task<ActionResult<string>> SendMessageWebApi()
    {
        var message = new Command();
        await messageSession.Send(message);
        return Ok("Message sent to endpoint using Web Api - refresh to send another");
    }
    #endregion
}