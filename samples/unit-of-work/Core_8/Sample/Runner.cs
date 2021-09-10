using System;
using System.Threading.Tasks;
using NServiceBus;

public class Runner
{
    public static async Task Run(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 's' to send a Success message");
        Console.WriteLine("Press 't' to send a Throw message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.S:

                    var successMessage = new MessageThatWillSucceed();
                    await endpointInstance.SendLocal(successMessage)
                        .ConfigureAwait(false);

                    break;
                case ConsoleKey.T:

                    var throwMessage = new MessageThatWillThrow();
                    await endpointInstance.SendLocal(throwMessage)
                        .ConfigureAwait(false);

                    break;
                default:
                {
                    return;
                }
            }
        }
    }

}