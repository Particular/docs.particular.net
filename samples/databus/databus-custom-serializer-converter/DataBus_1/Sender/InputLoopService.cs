using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Sender
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press 'D' to send a databus large message");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.D)
                {
                    await SendMessageLargePayload(messageSession);
                    continue;
                }
                break;
            }

        }
        static async Task SendMessageLargePayload(IMessageSession messageSession)
        {
            #region SendMessageLargePayload

            var message = new MessageWithLargePayload
            {
                SomeProperty = "This message contains a large blob that will be sent on the data bus",
                LargeBlob = new ClaimCheckProperty<byte[]>(new byte[1024 * 1024 * 5]) //5MB
            };
            await messageSession.Send("Samples.DataBus.Receiver", message);

            #endregion

            Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
        }
    }
}
