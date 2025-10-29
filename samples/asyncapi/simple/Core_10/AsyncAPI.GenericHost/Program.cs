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

Console.WriteLine("Press '1' to publish event one");
Console.WriteLine("Press '2' to publish event two");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2)
    {
        break;
    }

    var now = DateTime.UtcNow.ToString();

    if (key.Key == ConsoleKey.D1)
    {
        await messageSession.Publish(new SampleEventOne { SomeValue = now });

        Console.WriteLine($"Published event 1 with {now}.");
        continue;
    }

    await messageSession.Publish(new SampleEventTwo { SomeValue = now });

    Console.WriteLine($"Published event 2 with {now}.");
}

await host.StopAsync();