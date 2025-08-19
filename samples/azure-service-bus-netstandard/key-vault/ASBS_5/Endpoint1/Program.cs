using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

Console.Title = "Endpoint1";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.SendReply.Endpoint1");
endpointConfiguration.EnableInstallers();

string keyVaultUri = Environment.GetEnvironmentVariable("KeyVaultUri");
string connectionString = await new KeyVaultBasedConfigurationProvider(keyVaultUri).GetConfiguration("AzureServiceBusConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBusConnectionString' value. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

Console.WriteLine("Press any key, the application is starting");
Console.TreatControlCAsInput = true;
var input = Console.ReadKey();
if (input.Key == ConsoleKey.C && (input.Modifiers & ConsoleModifiers.Control) != 0)
{
    Environment.Exit(0);
}
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'enter' to send a message");
Console.WriteLine("Press any other key to exit");
while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var message = new Message1
    {
        Property = "Hello from Endpoint1"
    };
    await messageSession.Send("Samples.ASBS.SendReply.Endpoint2", message);
    Console.WriteLine("Message1 sent");
}

await host.StopAsync();
