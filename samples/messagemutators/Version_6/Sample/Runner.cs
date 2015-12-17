using System;
using System.Threading.Tasks;
using NServiceBus;

public class Runner
{
    public static async Task Run(IBusContext bus)
    {
        Console.WriteLine("Press 's' to send a valid message");
        Console.WriteLine("Press 'e' to send a failed message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.S:
                    #region SendingSmall
                    await bus.SendLocal(new CreateProductCommand
                    {
                        ProductId = "XJ128",
                        ProductName = "Milk",
                        ListPrice = 4,
                        // 7MB. MSMQ should throw an exception, but it will not since the buffer will be compressed 
                        // before it reaches MSMQ.
                        Image = new byte[1024 * 1024 * 7]
                    });
                    #endregion
                    break;
                case ConsoleKey.E:
                    try
                    {
                        #region SendingLarge
                        await bus.SendLocal(new CreateProductCommand
                        {
                            ProductId = "XJ128",
                            ProductName = "Really long product name",
                            ListPrice = 15,
                            // 7MB. MSMQ should throw an exception, but it will not since the buffer will be compressed 
                            // before it reaches MSMQ.
                            Image = new byte[1024 * 1024 * 7]
                        });
                        #endregion
                    }
                    catch
                    {
                        //so the console keeps on running   
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