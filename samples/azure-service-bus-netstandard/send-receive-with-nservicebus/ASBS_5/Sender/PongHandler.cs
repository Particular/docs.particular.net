using Shared;

namespace Sender;

public class PongHandler(ILogger<PongHandler> logger) : IHandleMessages<Pong>
{
    public Task Handle(Pong message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing Pong message: {Acknowledgement}", message.Acknowledgement);

        return Task.CompletedTask;
    }
}