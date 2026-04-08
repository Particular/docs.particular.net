using Shared;

Console.Title = "Sender";

#region ConfigureRabbit
var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.DelayedDelivery.Sender");
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString("host=localhost");
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
transport.Routing().RouteToEndpoint(typeof(MyCommand), "Samples.RabbitMQ.DelayedDelivery.Receiver");
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
await SendMessages(endpointInstance);
await endpointInstance.Stop();

static async Task SendMessages(IMessageSession messageSession)
{
    Console.WriteLine("Press [d] to send commands across multiple delay levels. Press [Esc] to exit.");

    while (true)
    {
        var input = Console.ReadKey();
        Console.WriteLine();

        switch (input.Key)
        {
            case ConsoleKey.D:
                #region SendDelayedMessages
                var delays = new[]
                {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(60),
                    TimeSpan.FromSeconds(360),
                    TimeSpan.FromSeconds(86400),
                };

                foreach (var delay in delays)
                {
                    var options = new SendOptions();
                    options.DelayDeliveryWith(delay);
                    options.SetDestination("Samples.RabbitMQ.DelayedDelivery.Receiver");
                    await messageSession.Send(new MyCommand(), options);
                    Console.WriteLine($"Sent command with delay of {delay}");
                }
                #endregion
                break;
            case ConsoleKey.Escape:
                return;
        }
    }
}
