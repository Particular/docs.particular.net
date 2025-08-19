using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
var random = new Random();

Console.Title = "AsbEndpoint";

var endpointConfiguration = new EndpointConfiguration("Samples.MessagingBridge.AsbEndpoint");
endpointConfiguration.EnableInstallers();
endpointConfiguration.UsePersistence<NonDurablePersistence>();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));

var builder = Host.CreateApplicationBuilder(args);
//builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.UseNServiceBus(endpointConfiguration);

var sendOptions = new SendOptions();
sendOptions.SetDestination("Samples.MessagingBridge.MsmqEndpoint");

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press Enter to send a command");
Console.WriteLine("Press [CTRL+C] to exit");

while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // Wait a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var key = Console.ReadKey().Key;
    if (key == ConsoleKey.Enter)
    {
        var prop = new string(Enumerable.Range(0, 3).Select(i => letters[random.Next(letters.Length)]).ToArray());
        await messageSession.Send(new MyCommand { Property = prop }, sendOptions);
        logger.LogInformation("Command with value '{Prop}' sent", prop);
    }
}

await host.StopAsync(); 
