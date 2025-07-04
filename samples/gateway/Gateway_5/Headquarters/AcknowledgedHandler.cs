using Microsoft.Extensions.Logging;
using Shared;

#region AcknowledgedHandler
public class AcknowledgedHandler(ILogger<AcknowledgedHandler> logger) : IHandleMessages<PriceUpdateAcknowledged>
{
    public Task Handle(PriceUpdateAcknowledged message, IMessageHandlerContext context)
    {
        logger.LogInformation("Price update received by: {BranchOffice}", message.BranchOffice);
        return Task.CompletedTask;
    }
}
#endregion