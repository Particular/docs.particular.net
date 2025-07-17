using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.MultiTenant.Sender");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
var random = new Random();
Console.WriteLine("Press A or B to publish a message (A and B are tenant IDs)");

var acceptableInput = new List<char> { 'A', 'B' };
while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.Escape)
    {
        break;
    }
    var uppercaseKey = char.ToUpperInvariant(key.KeyChar);

    if (acceptableInput.Contains(uppercaseKey))
    {
        var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
        var message = new OrderSubmitted
        {
            OrderId = orderId,
            Value = random.Next(100)
        };

        var options = new PublishOptions();
        options.SetHeader("tenant_id", uppercaseKey.ToString());

        await messageSession.Publish(message, options);
    }
    else
    {
        Console.WriteLine($"[{uppercaseKey}] is not a valid tenant identifier.");
    }
}

await host.StopAsync();