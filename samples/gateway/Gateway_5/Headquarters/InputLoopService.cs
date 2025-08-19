using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Shared;

namespace Headquarters
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

                string[] siteKeys = ["RemoteSite"];

                var priceUpdated = new PriceUpdated
                {
                    ProductId = 2,
                    NewPrice = 100.0,
                    ValidFrom = DateTime.Today,
                };
                await messageSession.SendToSites(siteKeys, priceUpdated);

                Console.WriteLine("Message sent, check the output in RemoteSite");
            }
        }
    }

}
