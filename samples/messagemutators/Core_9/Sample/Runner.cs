using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class Runner(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Press 's' to send a valid message");
        Console.WriteLine("Press 'e' to send a failed message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.S:

                    #region SendingSmall

                    var smallMessage = new CreateProductCommand
                    {
                        ProductId = "XJ128",
                        ProductName = "Milk",
                        ListPrice = 4,
                        Image = new byte[1024 * 1024 * 7]
                    };
                    await messageSession.SendLocal(smallMessage);

                    #endregion

                    break;
                case ConsoleKey.E:
                    try
                    {
                        #region SendingLarge

                        var largeMessage = new CreateProductCommand
                        {
                            ProductId = "XJ128",
                            ProductName = "Really long product name",
                            ListPrice = 15,
                            Image = new byte[1024 * 1024 * 7]
                        };
                        await messageSession.SendLocal(largeMessage);

                        #endregion
                    }
                    catch
                    {
                        // so the console keeps on running
                    }
                    break;
                default:
                    {
                        return;
                    }

            }

        }


    }

}
