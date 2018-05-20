using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus.SqlServer.HttpPassthrough;

#region Controller

[Route("SendMessage")]
public class PassThroughController : ControllerBase
{
    ISqlPassThrough sender;

    public PassThroughController(ISqlPassThrough sender)
    {
        this.sender = sender;
    }

    [HttpPost]
    public async Task Post(CancellationToken cancellation)
    {
        await sender.Send(HttpContext, cancellation)
            .ConfigureAwait(false);
    }
}

#endregion