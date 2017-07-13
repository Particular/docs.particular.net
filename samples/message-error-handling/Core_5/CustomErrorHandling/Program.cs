using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.CustomErrorHandling";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.CustomErrorHandling");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        #region Registering-Behavior

        busConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.ConfigureComponent<CustomFaultManager>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message that will throw an exception or \r\n" +
                              "Press [E] key to send a message failing with the custom exception.");
            Console.WriteLine("Press [ESC] key to exit");

            while (true)
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