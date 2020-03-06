using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sender
{
    class MessageSender : IHostedService
    {
        public MessageSender(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var messageSession = serviceProvider.GetService<IMessageSession>();

            Console.WriteLine("Sending a message...");

            var guid = Guid.NewGuid();
            Console.WriteLine($"Requesting to get data by id: {guid:N}");

            var message = new RequestMessage
            {
                Id = guid,
                Data = "String property value"
            };

            await messageSession.Send(message)
                .ConfigureAwait(false);

            Console.WriteLine("Message sent.");
            Console.WriteLine("Use 'docker-compose down' to stop containers.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        readonly IServiceProvider serviceProvider;
    }
}