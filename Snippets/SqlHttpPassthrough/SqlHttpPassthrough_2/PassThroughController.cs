using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus.SqlServer.HttpPassthrough;

#region Controller

[Route("SendMessage")]
public class PassthroughController : ControllerBase
{
    ISqlPassthrough sender;

    public PassthroughController(ISqlPassthrough sender)
    {
        this.sender = sender;
    }

    [HttpPost]
    public Task Post(CancellationToken cancellation)
    {
        return sender.Send(HttpContext, cancellation);
    }
}

#endregion