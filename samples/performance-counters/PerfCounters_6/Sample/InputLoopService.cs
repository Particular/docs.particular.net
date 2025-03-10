using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Sample
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
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
                for (var i = 0; i < 10; i++)
                {
                    var myMessage = new MyMessage();
                    await messageSession.SendLocal(myMessage);
                }
            }
        }
    }
}
