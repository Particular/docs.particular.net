using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;
using Shared;

Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#region ConfigureSenderCustomDataBusSerializer

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, BsonClaimCheckSerializer>();
claimCheck.BasePath(@"..\..\..\..\storage");

#endregion

endpointConfiguration.Conventions().DefiningClaimCheckPropertiesAs(prop => prop.Name.StartsWith("Large"));

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.StartAsync();

var messageSession = app.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press Enter to send a large message with claim check");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey(true);
    Console.WriteLine();

    if (key.Key == ConsoleKey.Enter)
    {
        await SendMessageLargePayload(messageSession);
        continue;
    }
    break;
}

await app.StopAsync();

static Task SendMessageLargePayload(IMessageSession messageSession)
{
    var measurements = GetMeasurements().ToArray();

    var message = new MessageWithLargePayload
    {
        SomeProperty = "This message contains a large collection that will be sent on the claim check",
        LargeData = measurements
    };
    Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
    return messageSession.Send("Samples.DataBus.Receiver", message);
}

static IEnumerable<Measurement> GetMeasurements()
{
    for (var i = 0; i < 10000; i++)
    {
        yield return new Measurement
        {
            Timestamp = DateTimeOffset.UtcNow,
            MeasurementName = $"Instrument {i}",
            MeasurementValue = i * 10m
        };
    }
}