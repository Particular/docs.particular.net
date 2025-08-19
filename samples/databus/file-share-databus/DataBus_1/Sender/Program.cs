using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using Shared;
using System.Threading.Tasks;


Console.Title = "Sender";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ClaimCheck.Sender");
var storagePath = new SolutionDirectoryFinder().GetDirectory("storage");

#region ConfigureDataBus

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
claimCheck.BasePath(storagePath);

#endregion

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetService<IMessageSession>();

Console.WriteLine("Press 'D' to send a claim check large message");
Console.WriteLine("Press 'N' to send a normal large message exceed the size limit and throw");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key == ConsoleKey.N)
    {
        await SendMessageTooLargePayload(messageSession);
        continue;
    }

    if (key.Key == ConsoleKey.D)
    {
        await SendMessageLargePayload(messageSession, storagePath);
        continue;
    }

    break;
}

await host.StopAsync();


static async Task SendMessageLargePayload(IMessageSession messageSession, string storagePath)
{
    #region SendMessageLargePayload

    var message = new MessageWithLargePayload
    {
        SomeProperty = "This message contains a large blob that will be sent via claim check",
        LargeBlob = new ClaimCheckProperty<byte[]>(new byte[1024 * 1024 * 5]) //5MB

    };
    await messageSession.Send("Samples.ClaimCheck.Receiver", message);

    #endregion

    Console.WriteLine($"Message sent, the payload is stored in: {storagePath}");
}

static async Task SendMessageTooLargePayload(IMessageSession messageSession)
{
    #region SendMessageTooLargePayload

    var message = new AnotherMessageWithLargePayload
    {
        LargeBlob = new byte[1024 * 1024 * 5] //5MB
    };
    await messageSession.Send("Samples.ClaimCheck.Receiver", message);

    #endregion
}