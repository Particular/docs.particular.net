using System;
using NServiceBus;

public class Runner
{
    public static void Run(IBus bus)
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

                    var messageThatWillSucceed = new MessageThatWillSucceed();
                    bus.SendLocal(messageThatWillSucceed);

                    break;
                case ConsoleKey.T:

                    var messageThatWillThrow = new MessageThatWillThrow();
                    bus.SendLocal(messageThatWillThrow);

                    break;
                default:
                {
                    return;
                }
            }
        }
    }

}