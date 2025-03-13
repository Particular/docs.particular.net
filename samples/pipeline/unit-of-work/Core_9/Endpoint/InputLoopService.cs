using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

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

                var options = new SendOptions();
                options.RouteToThisEndpoint();
                await messageSession.Send(new MyMessage(), options);
            }


        }
    }
}
