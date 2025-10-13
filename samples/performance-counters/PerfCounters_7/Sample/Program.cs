using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "PerfCounters";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.PerfCounters");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.SendFailedMessagesTo("error");

#region enable-counters
var performanceCounters = endpointConfiguration.EnableWindowsPerformanceCounters();
performanceCounters.EnableSLAPerformanceCounters(TimeSpan.FromSeconds(100));
#endregion

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [Enter] to send 10 messages with random sleep");
Console.WriteLine("Press any other key to exit");

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

