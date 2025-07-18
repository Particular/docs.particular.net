using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace SenderAndReceiver
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press 'Enter' to send a large message (>4MB)");
            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    await SendMessageLargePayload(messageSession);
                }
                else
                {
                    return;
                }
            }
        }
        static async Task SendMessageLargePayload(IMessageSession messageSession)
        {
            Console.WriteLine("Sending message...");

#pragma warning disable CS0618 // Type or member is obsolete
            var message = new MessageWithLargePayload
            {
                Description = "This message contains a large payload that will be sent on the Azure data bus",
                LargePayload = new DataBusProperty<byte[]>(new byte[1024 * 1024 * 5]) // 5MB
            };
#pragma warning restore CS0618 // Type or member is obsolete

            await messageSession.SendLocal(message);

            Console.WriteLine("Message sent.");
        }
    }
}
