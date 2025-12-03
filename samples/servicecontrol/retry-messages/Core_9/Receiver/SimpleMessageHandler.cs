using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class SimpleMessageHandler : IHandleMessages<SimpleMessage>
{
    public static bool FaultMode { get; set; } = true;

    private readonly ILogger<SimpleMessageHandler> _logger;

    public SimpleMessageHandler(ILogger<SimpleMessageHandler> logger) => this._logger = logger;

    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region ReceiverHandler

        _logger.LogInformation("Received message.");
        if (FaultMode)
        {
            throw new Exception("Simulated error.");
        }
        _logger.LogInformation("Successfully processed message.");
        return Task.CompletedTask;

        #endregion
    }
}