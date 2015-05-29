using System;
using NServiceBus;

public class Runner
{
    public static void Run(IBus bus)
    {
        Console.WriteLine("Press 's' to send a valid message, press 'e' to send a failed message. To exit, 'q'\n");

        string cmd;

        while ((cmd = Console.ReadKey().Key.ToString().ToLower()) != "q")
        {
            Console.WriteLine();
            switch (cmd)
            {
                case "s":
                    #region SendingSmall
                    bus.SendLocal(new CreateProductCommand
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
                case "e":
                    try
                    {
                        #region SendingLarge
                        bus.SendLocal(new CreateProductCommand
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
            }
        }
    }

}