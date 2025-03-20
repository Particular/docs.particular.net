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
            Console.WriteLine("Press Enter to send a databus large message");

            while (true)
            {
                var key = Console.ReadKey(true);
                Console.WriteLine();

                if (key.Key == ConsoleKey.Enter)
                {
                    await SendMessageLargePayload(messageSession);
                    continue;
                }
                break;
            }

        }
        static Task SendMessageLargePayload(IMessageSession messageSession)
        {
            var measurements = GetMeasurements().ToArray();

            var message = new MessageWithLargePayload
            {
                SomeProperty = "This message contains a large collection that will be sent on the data bus",
                LargeData = new ClaimCheckProperty<Measurement[]>(measurements)
            };
            Console.WriteLine(@"Message send, the payload is stored in: ..\..\..\storage");
            return messageSession.Send("Samples.DataBus.Receiver", message);
        }

        static IEnumerable<Measurement> GetMeasurements()
        {
            for (var i = 0; i < 10000; i++)
            {
                yield return new Measurement
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    MeasurementName = $"Instrument {i}",
                    MeasurementValue = i * 10m
                };
            }
        }
    }

}
