using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "CustomAuditTransport";

var endpointConfiguration = new EndpointConfiguration("Samples.CustomAuditTransport");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}
var transport = new AzureServiceBusTransport(connectionString, TopicTopology.Default);
endpointConfiguration.UseTransport(transport);
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.Recoverability().Immediate(i => i.NumberOfRetries(1));
endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(1));
endpointConfiguration.EnableInstallers();

Console.WriteLine("Starting...");

var builder = Host.CreateApplicationBuilder(args);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [s] to send a message, [e] to stimulate failure. Press any other key to exit.");

try
{
    while (true)
    {
        var key = Console.ReadKey();
        Console.WriteLine();

        if (key.Key != ConsoleKey.S && key.Key != ConsoleKey.E)
        {
            break;
        }

        if (key.Key == ConsoleKey.S)
        {
            await SendAuditMessageAsync(messageSession, false);
            continue;
        }

        await SendAuditMessageAsync(messageSession, true);
    }
}
catch (Exception ex) when (ex is not OperationCanceledException)
{
    Console.WriteLine($"\nUnexpected error: {ex.Message}");
}

await host.StopAsync();

return;

static async Task SendAuditMessageAsync(IMessageSession messageSession, bool error)
{
    var auditThisMessage = new AuditThisMessage
    {
        Content = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} - see you in the audit queue!",
        Error = error
    };
    await messageSession.SendLocal(auditThisMessage);
    Console.WriteLine("\nMessage sent to local endpoint for auditing.");
}