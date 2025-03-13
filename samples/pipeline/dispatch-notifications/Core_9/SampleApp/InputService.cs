using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace SampleApp
{
    public class InputService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Console.WriteLine("Press any key to send a message");

                while (Console.ReadKey(true).Key != ConsoleKey.Escape)
                {
                    await messageSession.SendLocal(new SomeMessage());
                }

            }

        }
    }
}
