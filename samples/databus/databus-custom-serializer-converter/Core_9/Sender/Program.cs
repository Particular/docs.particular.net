using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;

Console.Title = "Sender";

var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#pragma warning disable CS0618 // Type or member is obsolete
#region ConfigureDataBus

var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
dataBus.BasePath(@"..\..\..\..\storage");

#endregion
#pragma warning restore CS0618 // Type or member is obsolete

#region CustomJsonSerializerOptions
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.Converters.Add(new DatabusPropertyConverterFactory());
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);
#endregion

endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'D' to send a databus large message");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.D)
    {
        await SendMessageLargePayload(messageSession);
        continue;
    }
    break;
}

await host.StopAsync();
return;

static async Task SendMessageLargePayload(IMessageSession messageSession)
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region SendMessageLargePayload

    var message = new MessageWithLargePayload
    {
        SomeProperty = "This message contains a large blob that will be sent on the data bus",
        LargeBlob = new DataBusProperty<byte[]>(new byte[1024 * 1024 * 5]) //5MB
    };
    await messageSession.Send("Samples.DataBus.Receiver", message);

    #endregion
#pragma warning restore CS0618 // Type or member is obsolete

    Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
}