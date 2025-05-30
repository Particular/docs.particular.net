using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus.TransactionalSession;

[ApiController]
[Route("")]
public class SendMessageController : Controller
{
    [HttpGet]
    public async Task<string> Get([FromServices] ITransactionalSession messageSession)
    {
        var id = Guid.NewGuid().ToString();

       var message = new MyMessage { MessageText = "Hello from Sender" };

        await messageSession.Send(message);

        return "Message sent";
    }
}