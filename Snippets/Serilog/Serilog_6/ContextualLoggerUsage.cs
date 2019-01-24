using System.Threading.Tasks;
using NServiceBus;

#region ContextualLoggerUsage

public class TheHandler : IHandleMessages<TheMessage>
{
    public Task Handle(TheMessage message, IMessageHandlerContext context)
    {
        var logger = context.Logger();
        logger.Information("Hello from {@Handler}.", nameof(TheHandler));
        return Task.CompletedTask;
    }
}
#endregion
