using System.Threading.Tasks;
using NServiceBus;
using Messages;
using System;


class Program
{
  public static async Task Main(string[] args)
  {
    var endpointConfiguration = new EndpointConfiguration("Samples.DynamoDB.Lambda.ClientUI");
    endpointConfiguration.EnableInstallers();
    endpointConfiguration.SendFailedMessagesTo("Samples.DynamoDB.Lambda.Error");

    var transport = endpointConfiguration.UseTransport<SqsTransport>();
    var routing = transport.Routing();

    routing.RouteToEndpoint(typeof(PlaceOrder), "Samples.DynamoDB.Lambda.Sales");

    var endpoint = await Endpoint.Start(endpointConfiguration);


    Console.WriteLine();
    Console.WriteLine("Press Enter to place an order. Press Q to quit.");

    while (true)
    {
      var pressedKey = Console.ReadKey(true);

      switch (pressedKey.Key)
      {
        case ConsoleKey.Enter:
          {
            var orderId = Guid.NewGuid().ToString("N");
            await endpoint.Send(new PlaceOrder() { OrderId = orderId }).ConfigureAwait(false);
            Console.WriteLine($"Order {orderId} was placed.");

            break;
          }
        case ConsoleKey.Q:
          {
            await endpoint.Stop();
            return;
          }
      }
    }
  }
}




