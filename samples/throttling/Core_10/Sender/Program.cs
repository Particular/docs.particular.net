using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sender";

var endpointConfiguration = new EndpointConfiguration("Samples.Throttling.Sender");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'Enter' to send messages");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    #region Sending
    Console.WriteLine("Sending messages...");
    for (var i = 0; i < 100; i++)
    {
        var searchGitHub = new SearchGitHub
        {
            Repository = "NServiceBus",
            Owner = "Particular",
            Branch = "master"
        };
        await messageSession.Send("Samples.Throttling.Limited", searchGitHub);
    }
    #endregion

    Console.WriteLine("Messages sent.");
}

await host.StopAsync();