using Microsoft.Extensions.Logging;
using Shared;

#region AcknowledgedHandler
public class AcknowledgedHandler : IHandleMessages<PriceUpdateAcknowledged>
{
    private static readonly ILogger<AcknowledgedHandler> logger =
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    }).CreateLogger<AcknowledgedHandler>();

    public Task Handle(PriceUpdateAcknowledged message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Price update received by: {message.BranchOffice}");
        return Task.CompletedTask;
    }
}
#endregion