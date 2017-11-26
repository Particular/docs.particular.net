using System;
using System.Threading.Tasks;
using NServiceBus;

public class Runner
{
    public static async Task Run(IEndpointInstance endpointInstance)
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
                    await endpointInstance.SendLocal(smallMessage)
                        .ConfigureAwait(false);

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
                        await endpointInstance.SendLocal(largeMessage)
                            .ConfigureAwait(false);

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
