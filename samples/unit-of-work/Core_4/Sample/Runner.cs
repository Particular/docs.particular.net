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

                    #region SendingSuccessMessage

                    var messageThatWillSucceed = new MessageThatWillSucceed();
                    bus.SendLocal(messageThatWillSucceed);

                    #endregion

                    break;
                case ConsoleKey.T:

                    #region SendingThrowMessage

                    var messageThatWillThrow = new MessageThatWillThrow();
                    bus.SendLocal(messageThatWillThrow);

                    #endregion

                    break;
                default:
                {
                    return;
                }
            }
        }
    }

}