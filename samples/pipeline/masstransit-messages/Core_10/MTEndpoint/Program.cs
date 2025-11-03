using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Messages.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTEndpoint;

Console.Title = "MT Endpoint";

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MessageConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", rabbitConfig =>
        {
            rabbitConfig.Username("guest");
            rabbitConfig.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });

    x.AddConfigureEndpointsCallback((name, cfg) =>
    {
        if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
            rmq.SetQuorumQueue(3);
    });
});

var host = builder.Build();

await host.StartAsync();

var bus = host.Services.GetRequiredService<IBus>();

Console.WriteLine("Sending messages on a one second interval. Press [CTRL] + [C] to exit");

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
        #region MassTransitPublish
        while (!cts.Token.IsCancellationRequested)
        {
            await bus.Publish(new MassTransitEvent { Text = $"The time is {DateTimeOffset.Now}" });
            await Task.Delay(1000, cts.Token);
        }
        #endregion
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();