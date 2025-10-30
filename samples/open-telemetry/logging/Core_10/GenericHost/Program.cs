using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var endpointName = "Samples.OpenTelemetry.MyEndpoint";

var builder = Host.CreateApplicationBuilder(args);

#region otel-config

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resourceBuilder => resourceBuilder.AddService(endpointName))
    .WithTracing(traceBuilder =>
    {
        traceBuilder.AddSource("NServiceBus.*");
        traceBuilder.AddConsoleExporter();
    });

#endregion

#region otel-logging

builder.Logging.AddOpenTelemetry(loggingOptions =>
{
    loggingOptions.IncludeFormattedMessage = true;
    loggingOptions.IncludeScopes = true;
    loggingOptions.ParseStateValues = true;
    loggingOptions.AddConsoleExporter();
});

#endregion

#region otel-nsb-config

var endpointConfiguration = new EndpointConfiguration(endpointName);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.EnableOpenTelemetry();

builder.UseNServiceBus(endpointConfiguration);

#endregion

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Sending messages on an increasing interval. Press [CTRL] + [C] to exit");

using (var cts = new CancellationTokenSource())
{
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Cancellation Requested...");
        cts.Cancel();
        e.Cancel = true;
    };

    try
    {
        var number = 0;

        while (!cts.Token.IsCancellationRequested)
        {
            await messageSession.SendLocal(new MyMessage { Number = number++ }, cts.Token);

            await Task.Delay(1000, cts.Token);
        }
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();