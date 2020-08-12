using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Receiver
{
    public class RequestMessageHandler
        : IHandleMessages<RequestMessage>
    {
        readonly ILogger logger;

        public RequestMessageHandler(ILogger<RequestMessageHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(RequestMessage message, IMessageHandlerContext context)
        {
            logger.LogInformation($"Request received with description: {message.Data}");

            var response = new ResponseMessage
            {
                Id = message.Id,
                Data = message.Data
            };
            return context.Reply(response);
        }
    }
}