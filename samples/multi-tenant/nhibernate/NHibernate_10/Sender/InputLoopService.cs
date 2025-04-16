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
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var random = new Random();

            Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs)");
            Console.WriteLine("Press Escape to exit");
            var acceptableInput = new List<char> { 'A', 'B' };

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                var uppercaseKey = char.ToUpperInvariant(key.KeyChar);

                if (acceptableInput.Contains(uppercaseKey))
                {
                    var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                    var message = new OrderSubmitted
                    {
                        OrderId = orderId,
                        Value = random.Next(100)
                    };

                    var options = new PublishOptions();
                    options.SetHeader("tenant_id", uppercaseKey.ToString());

                    await messageSession.Publish(message, options);

                    Console.WriteLine($"Submitted order {message.OrderId} for tenant {uppercaseKey}");
                }
                else
                {
                    Console.WriteLine($"[{uppercaseKey}] is not a valid tenant identifier.");
                }
            }


        }
    }
}
