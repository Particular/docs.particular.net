using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomErrorHandling";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.CustomErrorHandling");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();

        #region Registering-Behavior
        configure.Configurer.ConfigureComponent<CustomFaultManager>(DependencyLifecycle.InstancePerCall);
        #endregion

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press enter to send a message that will throw an exception or \r\n" +
                              "Press [E] key to send a message failing with the custom exception.");
            Console.WriteLine("Press [ESC] key to exit");

            while(true)
            {
                var input = Console.ReadKey();

                var myMessage = new MyMessage
                {
                    Id = Guid.NewGuid(),
                    ThrowCustomException = input.Key == ConsoleKey.E
                };

                if (input.Key == ConsoleKey.Escape)
                {
                    break;
                }
                bus.SendLocal(myMessage);
            }
        }
    }
}