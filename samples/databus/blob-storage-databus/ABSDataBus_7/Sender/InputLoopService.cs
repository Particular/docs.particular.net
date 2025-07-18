using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sender
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

            #region SendMessageLargePayload

            var message = new MessageWithLargePayload
            {
                Description = "This message contains a large payload that will be sent on the Azure data bus",
                LargePayload = new ClaimCheckProperty<byte[]>(new byte[1024 * 1024 * 5]) // 5MB
            };
            await messageSession.Send("Samples.AzureBlobStorageDataBus.Receiver", message);

            #endregion

            Console.WriteLine("Message sent.");
        }
    }
}
