using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "UnitOfWorkEndpoint";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.UnitOfWork.Endpoint");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region configuration

var pipeline = endpointConfiguration.Pipeline;
pipeline.Register(new MyUowBehavior(), "Manages the session");
builder.Services.AddScoped<IMySession, MySession>();
builder.Services.AddScoped<MySession, MySession>();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

var key = default(ConsoleKeyInfo);

Console.WriteLine("Press any key to send messages, 'q' to exit");
while (key.KeyChar != 'q')
{
    key = Console.ReadKey();

    for (var i = 1; i < 4; i++)
    {
        var options = new SendOptions();
        options.SetHeader("tenant", "tenant" + i);
        options.RouteToThisEndpoint();
        await messageSession.Send(new MyMessage(), options);
    }
}

await host.StopAsync();
