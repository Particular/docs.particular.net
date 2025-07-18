using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Lambda.ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendFailedMessagesTo("Samples-DynamoDB-Lambda-Error");

var transport = endpointConfiguration.UseTransport<SqsTransport>();
var routing = transport.Routing();
routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.DynamoDB.Lambda.Sales");

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine();
Console.WriteLine("Press [Enter] to place an order. Press [Esc] to quit.");

while (true)
{
    var pressedKey = Console.ReadKey(true);

    switch (pressedKey.Key)
    {
        case ConsoleKey.Enter:
            {
                var orderId = Guid.NewGuid().ToString("N");
                await messageSession.Send(new PlaceOrder() { OrderId = orderId });
                Console.WriteLine($"Order {orderId} was placed.");

                break;
            }
        case ConsoleKey.Escape:
            {
                return;
            }
    }
}




