using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Sample
{
    public class InputService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press 'Enter' to send a Message");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    await SendMessage(messageSession);
                    continue;
                }
                return;
            }
        }
        static Task SendMessage(IMessageSession messageSession)
        {
            Console.WriteLine();
            Console.WriteLine("Message sent");
            var message = new Message();
            return messageSession.SendLocal(message);
        }
    }
}
