var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Lambda.ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendFailedMessagesTo("Samples-DynamoDB-Lambda-Error");

var transport = endpointConfiguration.UseTransport<SqsTransport>();
var routing = transport.Routing();
routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.DynamoDB.Lambda.Sales");

var endpoint = await Endpoint.Start(endpointConfiguration);

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
                await endpoint.Send(new PlaceOrder() { OrderId = orderId });
                Console.WriteLine($"Order {orderId} was placed.");

                break;
            }
        case ConsoleKey.Escape:
            {
                await endpoint.Stop();
                return;
            }
    }
}