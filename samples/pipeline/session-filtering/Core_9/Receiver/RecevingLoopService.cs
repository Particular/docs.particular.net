using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Receiver
{

    public class RecevingLoopService(RotatingSessionKeyProvider sessionKeyProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PrintMenu(sessionKeyProvider);

            while (!stoppingToken.IsCancellationRequested)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.C:
                        sessionKeyProvider.NextKey();
                        PrintMenu(sessionKeyProvider);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        static void PrintMenu(ISessionKeyProvider sessionKeyProvider)
        {
            Console.Clear();
            Console.WriteLine($"Session Key: {sessionKeyProvider.SessionKey}");
            Console.WriteLine("C)   Change Session Key");
            Console.WriteLine("ESC) Close");
        }
    }
}
