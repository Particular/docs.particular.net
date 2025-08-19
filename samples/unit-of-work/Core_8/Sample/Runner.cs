using System;
using System.Threading.Tasks;
using NServiceBus;

public class Runner
{
    public static async Task Run(IEndpointInstance endpointInstance)
    {
        await Console.Out.WriteAsync("Press 's' to send a Success message");
        await Console.Out.WriteAsync("Press 't' to send a Throw message");
        await Console.Out.WriteAsync("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync();
            switch (key.Key)
            {
                case ConsoleKey.S:

                    var successMessage = new MessageThatWillSucceed();
                    await endpointInstance.SendLocal(successMessage);

                    break;
                case ConsoleKey.T:

                    var throwMessage = new MessageThatWillThrow();
                    await endpointInstance.SendLocal(throwMessage);

                    break;
                default:
                    return;
            }
        }
    }
}