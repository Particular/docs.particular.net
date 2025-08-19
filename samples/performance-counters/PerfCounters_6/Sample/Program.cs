
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample;

Console.Title = "PerfCounters";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();
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
await builder.Build().RunAsync();
