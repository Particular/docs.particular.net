using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
var builder = Host.CreateApplicationBuilder(args);

Console.Title = "UnitOfWorkEndpoint";

var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region configuration
builder.Services.AddSingleton<MySessionProvider>();

// Then later get it from the service provider when needed
var serviceProvider = builder.Services.BuildServiceProvider();
var sessionProvider = serviceProvider.GetRequiredService<MySessionProvider>();


var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(new MyUowBehavior(sessionProvider), "Manages the session");
#endregion



builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
Console.WriteLine("Press any key to send messages, 'q' to exit");
while (true)
{
    var key = Console.ReadKey();

    if (key.Key == ConsoleKey.Q)
    {
        break;
    }

    var options = new SendOptions();
    options.RouteToThisEndpoint();
    await messageSession.Send(new MyMessage(), options);

    Console.WriteLine("MyMessage sent. Press any key to send another message, 'q' to exit.");
}

await host.StopAsync();