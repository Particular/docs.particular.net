using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Endpoint
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var key = default(ConsoleKeyInfo);

            Console.WriteLine("Press any key to send messages, 'q' to exit");
            while (key.KeyChar != 'q')
            {
                key = Console.ReadKey();

                for (var i = 1; i < 4; i++)
                {
                    var options = new SendOptions();
                    options.SetHeader("tenant", "tenant" + i);
                    options.RouteToThisEndpoint();
                    await messageSession.Send(new MyMessage(), options);
                }
            }

        }
    }
}
