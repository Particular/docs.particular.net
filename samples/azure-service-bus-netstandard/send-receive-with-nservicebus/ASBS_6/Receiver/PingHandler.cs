using Shared;

namespace Receiver;

public class PingHandler(ILogger<PingHandler> logger) : IHandleMessages<Ping>
{
    public async Task Handle(Ping message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing Ping message #{Round}", message.Round);

        var reply = new Pong { Acknowledgement = $"Ping #{message.Round} processed at {DateTimeOffset.UtcNow:s}" };

        await context.Reply(reply);

        // throw new Exception("BOOM");
    }
}
