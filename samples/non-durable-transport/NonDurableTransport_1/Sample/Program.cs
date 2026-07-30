using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Non-Durable Transport Sample";

var builder = Host.CreateApplicationBuilder(args);

#region sample-config
builder.Services.AddSingleton(new NonDurableBroker());

var sagaEndpoint = new EndpointConfiguration("Samples.NonDurable.Saga");
sagaEndpoint.UsePersistence<NonDurablePersistence>();
sagaEndpoint.UseSerialization<SystemJsonSerializer>();
sagaEndpoint.AssemblyScanner().Disable = true;
sagaEndpoint.AddSaga<OrderSaga>();
sagaEndpoint.AddHandler<PaymentCompletedHandler>();

var sagaTransport = new NonDurableTransport(new NonDurableTransportOptions
{
    InlineExecution = new InlineExecutionOptions()
});
var sagaRouting = sagaEndpoint.UseTransport(sagaTransport);
sagaRouting.RouteToEndpoint(typeof(ProcessPayment), "Samples.NonDurable.Payment");

var paymentEndpoint = new EndpointConfiguration("Samples.NonDurable.Payment");
paymentEndpoint.UsePersistence<NonDurablePersistence>();
paymentEndpoint.UseSerialization<SystemJsonSerializer>();
paymentEndpoint.AssemblyScanner().Disable = true;
paymentEndpoint.AddHandler<ProcessPaymentHandler>();

var paymentTransport = new NonDurableTransport(new NonDurableTransportOptions());
paymentEndpoint.UseTransport(paymentTransport);
#endregion

builder.Services.AddNServiceBusEndpoint(sagaEndpoint, "Saga");
builder.Services.AddNServiceBusEndpoint(paymentEndpoint, "Payment");

using var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredKeyedService<IMessageSession>("Saga");

Console.WriteLine();
Console.WriteLine("Press 'Enter' to send a PlaceOrder message");
Console.WriteLine("Press any other key to exit");

while (true)
{
    if (Console.ReadKey().Key != ConsoleKey.Enter)
    {
        break;
    }
    var orderId = Guid.NewGuid();
    await messageSession.SendLocal(new PlaceOrder { OrderId = orderId });
    Console.WriteLine($"Sent PlaceOrder with OrderId {orderId}");
}

await host.StopAsync();
