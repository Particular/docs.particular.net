using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var config = new EndpointConfiguration(EndpointNames.WorkGenerator);

config.UsePersistence<LearningPersistence>();
config.UseSerialization<SystemJsonSerializer>();
config.AuditProcessedMessagesTo("audit");
config.SendFailedMessagesTo("error");

var transport = config.UseTransport<LearningTransport>();
var routing = transport.Routing();
RoutingHelper.ApplyDefaultRouting(routing);

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(config);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Started.");
Console.WriteLine("Press 'S' to start a new process or [Enter] to exit.");

await RunInputLoop(messageSession);

await host.StopAsync();

static async Task RunInputLoop(IMessageSession messageSession)
{
    while (true)
    {
        var pressedKey = Console.ReadKey();
        switch (pressedKey.Key)
        {
            case ConsoleKey.Enter:
            {
                return;
            }
            case ConsoleKey.S:
            {
                await StartSaga(messageSession);
                break;
            }
        }
    }
}

static async Task StartSaga(IMessageSession session)
{
    var id = Guid.NewGuid();
    var count = Random.Shared.Next(500, 1000);

    await session.SendLocal(new StartProcessing
    {
        ProcessId = id,
        WorkCount = count
    });
    Console.WriteLine($"Started process '{id}' with '{count}' work orders.");
}