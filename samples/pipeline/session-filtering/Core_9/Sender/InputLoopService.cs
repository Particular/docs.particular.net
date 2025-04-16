using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sender
{
    public class InputLoopService(IMessageSession messageSession, RotatingSessionKeyProvider sessionKeyProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PrintMenu(sessionKeyProvider);

            var index = 1;

            while (true)
            {

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.C:
                        sessionKeyProvider.NextKey();
                        PrintMenu(sessionKeyProvider);
                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        await messageSession.Send(new SomeMessage { Counter = index });
                        Console.WriteLine($"Sent message {index++}");
                        break;
                }
            }

        }
        static void PrintMenu(ISessionKeyProvider sessionKeyProvider)
        {
            Console.Clear();
            Console.WriteLine($"Session Key: {sessionKeyProvider.SessionKey}");
            Console.WriteLine("C)   Change Session Key");
            Console.WriteLine("ESC) Close");
            Console.WriteLine("any other key to send a message");
        }
    }

}
