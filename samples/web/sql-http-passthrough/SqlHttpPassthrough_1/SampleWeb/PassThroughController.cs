using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus.SqlServer.HttpPassthrough;

#region PassThroughController

[Route("SendMessage")]
public class PassThroughController : ControllerBase
{
    ISqlPassThrough sender;

    public PassThroughController(ISqlPassThrough sender)
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