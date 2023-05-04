using Messages;

using NServiceBus;

var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Lambda.ClientUI");
endpointConfiguration.EnableInstallers();
endpointConfiguration.SendFailedMessagesTo("Samples.DynamoDB.Lambda.Error");

var transport = endpointConfiguration.UseTransport<SqsTransport>();
var routing = transport.Routing();

routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.DynamoDB.Lambda.Sales");

var persistence = endpointConfiguration.UsePersistence<DynamoPersistence>();

persistence.UseSharedTable(new TableConfiguration()
{
  TableName = "Samples.DynamoDB.Lambda",
});

var endpoint = await Endpoint.Start(endpointConfiguration);

bool running = true;

Console.WriteLine();
Console.WriteLine("Press Enter to place an order. Press C to cancel the last order sent. Press Q to quit.");

string? lastOrderSent = null;

while (running)
{
  var pressedKey = Console.ReadKey(true);

  switch (pressedKey.Key)
  {
    case ConsoleKey.Enter:
      {
        lastOrderSent = Guid.NewGuid().ToString("N");
        await endpoint.Send(new PlaceOrder() { OrderId = lastOrderSent });
        Console.WriteLine($"Order {lastOrderSent} was placed.");

        break;
      }
    case ConsoleKey.C:
      {
        if (lastOrderSent == null)
        {
          Console.WriteLine("No orders to cancel, press Enter to place an order.");
          break;
        }

        await endpoint.Send(new CancelOrder() { OrderId = lastOrderSent });
        Console.WriteLine($"Cancelling order {lastOrderSent}");
        break;
      }
    case ConsoleKey.Q:
      {
        await endpoint.Stop();
        running = false;
        break;
      }
  }
}



