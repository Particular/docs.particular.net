using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Avro";

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Avro");

endpointConfiguration.UseSerialization<AvroSerializer>();

#endregion

endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.Pipeline.Register(typeof(MessageBodyLogger), "Logs the message body received");

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine();
Console.WriteLine("Press 'Enter' to send a message");
Console.WriteLine("Press any other key to exit");

while (true)
{
    if (Console.ReadKey().Key != ConsoleKey.Enter)
    {
        break;
    }

    #region message

    var message = new CreateOrder
    {
        OrderId = 9,
        Date = DateTime.Now,
        CustomerId = 12,
        OrderItems =
        [
            new OrderItem
            {
                ItemId = 6,
                Quantity = 2
            },

            new OrderItem
            {
                ItemId = 5,
                Quantity = 4
            }
        ]
    };

    await messageSession.SendLocal(message);

    #endregion

    Console.WriteLine("Message Sent");
}

await host.StopAsync();