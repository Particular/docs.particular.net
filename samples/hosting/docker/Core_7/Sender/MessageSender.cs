using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Sender
{
    class MessageSender : IHostedService
    {
        public MessageSender(ILogger<MessageSender> logger, IMessageSession messageSession)
        {
            this.logger = logger;
            this.messageSession = messageSession;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();

            var message = new RequestMessage
            {
                Id = guid,
                Data = "String property value"
            };

            await messageSession.Send(message)
                .ConfigureAwait(false);

            logger.LogInformation($"Message sent, requesting to get data by id: {guid:N}");
            logger.LogInformation("Use 'docker-compose down' to stop containers.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        readonly ILogger logger;
        readonly IMessageSession messageSession;
    }
}