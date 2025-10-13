using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

Console.Title = "Sender";
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#region ConfigureDataBus

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
claimCheck.BasePath(@"..\..\..\..\storage");

#endregion

#region CustomJsonSerializerOptions
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.Converters.Add(new ClaimCheckPropertyConverterFactory());
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
    #region SendMessageLargePayload

    var message = new MessageWithLargePayload
    {
        SomeProperty = "This message contains a large blob that will be sent on the data bus",
        LargeBlob = new ClaimCheckProperty<byte[]>(new byte[1024 * 1024 * 5]) //5MB
    };
    await messageSession.Send("Samples.DataBus.Receiver", message);

    #endregion

    Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
}