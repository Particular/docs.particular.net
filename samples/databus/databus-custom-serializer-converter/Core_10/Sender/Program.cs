using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.ClaimCheck;

Console.Title = "Sender";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.DataBus.Sender");

#region ConfigureClaimCheck

var claimCheck = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();
claimCheck.BasePath(@"..\..\..\..\storage");

#endregion

#region ClaimCheckConventions
endpointConfiguration.Conventions().DefiningClaimCheckPropertiesAs(prop => prop.Name.StartsWith("Large"));
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.StartAsync();

var messageSession = app.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'D' to send a large message with claim check");

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

await app.StopAsync();

static async Task SendMessageLargePayload(IMessageSession messageSession)
{
    #region SendMessageLargePayload

    var message = new MessageWithLargePayload
    {
        SomeProperty = "This message contains a large blob that will be sent on the claim check",
        LargeBlob = new byte[1024 * 1024 * 5] //5MB
    };
    await messageSession.Send("Samples.DataBus.Receiver", message);

    #endregion

    Console.WriteLine(@"Message sent, the payload is stored in: ..\..\..\storage");
}
