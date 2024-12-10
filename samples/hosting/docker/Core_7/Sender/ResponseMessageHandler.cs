using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Sender
{
    public class ResponseMessageHandler
        : IHandleMessages<ResponseMessage>
    {
        readonly ILogger logger;

        public ResponseMessageHandler(ILogger<ResponseMessageHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(ResponseMessage message, IMessageHandlerContext context)
        {
            logger.LogInformation($"Response received with description: {message.Data}");
            return Task.CompletedTask;
        }
    }
}