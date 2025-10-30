using AsyncAPI.Feature;
using Neuroglia.AsyncApi;

var builder = Host.CreateApplicationBuilder(args);

Console.Title = "AsyncAPI Generic Host";

#region GenericHostAddNeurogliaAsyncApi
builder.Services.AddAsyncApi();
#endregion

var endpointConfiguration = new EndpointConfiguration("AsyncAPI.GenericHost");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

#region GenericHostEnableAsyncAPIOnNSB
endpointConfiguration.EnableAsyncApiSupport();
#endregion

builder.UseNServiceBus(endpointConfiguration);

#region GenericHostAddSchemaWriter
builder.Services.AddHostedService<AsyncAPISchemaWriter>();
#endregion

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press '1' to publish event 1");
Console.WriteLine("Press '2' to publish event 2");
Console.WriteLine("Press 's' to send local message");
Console.WriteLine("Press any other key to exit");

var number = 0;
var continueInputLoop = true;

while (continueInputLoop)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    var now = DateTime.UtcNow.ToString();
    switch (key.Key)
    {
        case ConsoleKey.D1:
            await messageSession.Publish(new FirstEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

            Console.WriteLine($"Published event 1 with {now}.");
            break;
        case ConsoleKey.D2:
            await messageSession.Publish(new SecondEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

            Console.WriteLine($"Published event 2 with {now}.");
            break;
        case ConsoleKey.S:
            await messageSession.SendLocal(new MessageBeingSent { Number = number++ });

            Console.WriteLine($"Sent message with {number}.");
            break;
        default:
            continueInputLoop = false;
            break;
    }
}

await host.StopAsync();