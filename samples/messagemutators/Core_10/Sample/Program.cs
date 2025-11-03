using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.MessageMutator;

Console.Title = "MessageMutators";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.MessageMutators");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region ComponentRegistration
builder.Services.AddSingleton<IMutateIncomingMessages, ValidationMessageMutator>();
builder.Services.AddSingleton<IMutateOutgoingMessages, ValidationMessageMutator>();

// Add the compression mutator
builder.Services.AddSingleton<IMutateIncomingTransportMessages, TransportMessageCompressionMutator>();
builder.Services.AddSingleton<IMutateOutgoingTransportMessages, TransportMessageCompressionMutator>();

#endregion

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 's' to send a valid message");
Console.WriteLine("Press 'e' to send a failed message");
Console.WriteLine("Press any key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.S && key.Key != ConsoleKey.E)
    {
        break;
    }

    if (key.Key == ConsoleKey.S)
    {
        #region SendingSmall

        var smallMessage = new CreateProductCommand
        {
            ProductId = "XJ128",
            ProductName = "Milk",
            ListPrice = 4,
            Image = new byte[1024 * 1024 * 7]
        };
        await messageSession.SendLocal(smallMessage);

        #endregion
    }
    else if (key.Key == ConsoleKey.E)
    {
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
            await messageSession.SendLocal(largeMessage);

            #endregion
        }
        catch
        {
            // so the console keeps on running
        }
    }
}

await host.StopAsync();