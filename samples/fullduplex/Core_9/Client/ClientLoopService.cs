using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Client
{
  
    public class ClientLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }
                var guid = Guid.NewGuid();
                Console.WriteLine($"Requesting to get data by id: {guid:N}");

                var message = new RequestDataMessage
                {
                    DataId = guid,
                    String = "String property value"
                };
                await messageSession.Send("Samples.FullDuplex.Server", message);
            }


        }
    }
}
