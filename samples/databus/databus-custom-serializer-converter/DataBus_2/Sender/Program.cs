using NServiceBus;
using Shared;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#region ConfigureDataBus

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
claimCheck.BasePath(SolutionDirectoryFinder.Find("storage"));

#endregion

#region CustomJsonSerializerOptions
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.Converters.Add(new ClaimCheckPropertyConverterFactory());
endpointConfiguration.UseSerialization<SystemJsonSerializer>().Options(jsonSerializerOptions);
#endregion

endpointConfiguration.UseTransport<LearningTransport>();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'D' to send a databus large message");
Console.WriteLine("Press any other key to exit");

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

    Console.WriteLine($@"Message sent, the payload is stored in: {SolutionDirectoryFinder.Find("storage")}");
}