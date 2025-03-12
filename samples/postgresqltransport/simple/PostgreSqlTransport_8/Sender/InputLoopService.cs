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
            Console.WriteLine("Press [c] to send a command, or [e] to publish an event. Press [Esc] to exit.");
            while (true)
            {
                var input = Console.ReadKey();
                Console.WriteLine();

                switch (input.Key)
                {
                    case ConsoleKey.C:
                        await messageSession.Send(new MyCommand());
                        break;
                    case ConsoleKey.E:
                        await messageSession.Publish(new MyEvent());
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

        }
    }
}
