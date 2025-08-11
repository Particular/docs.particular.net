using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "PerfCounters";
var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<InputLoopService>();
var endpointConfiguration = new EndpointConfiguration("Samples.PerfCounters");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.SendFailedMessagesTo("error");

#region enable-counters
var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromSeconds(100));
#endregion

Console.WriteLine("Press enter to send 10 messages with random sleep");
Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    for (var i = 0; i < 10; i++)
    {
        var myMessage = new MyMessage();
        await messageSession.SendLocal(myMessage);
    }

    Console.WriteLine("10 messages sent.");
}

await host.StopAsync();

