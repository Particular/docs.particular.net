using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Publisher.Contracts;

class Program
{
    static async Task Main()
    {
        Console.Title = "Producer";

        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Producer");
        endpointConfiguration.UseTransport(new LearningTransport());
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await host.StartAsync();

        await Start(messageSession);

        await host.StopAsync();
    }

    static async Task Start(IMessageSession messageSession)
    {
        Console.WriteLine("Press 'p' to publish event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    var myEvent = new MyEvent
                    {
                        Consumer1Property = "Consumer1Info",
                        Consumer2Property = "Consumer2Info"
                    };
                    await messageSession.Publish(myEvent);

                    continue;
            }

            return;
        }
    }
}