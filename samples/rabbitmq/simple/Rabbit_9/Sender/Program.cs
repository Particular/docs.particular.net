using Shared;

Console.Title = "SimpleSender";

#region ConfigureRabbit
var endpointConfiguration = new EndpointConfiguration("Samples.RabbitMQ.SimpleSender");

var rabbitMqTransport = new RabbitMQTransport(
    routingTopology: RoutingTopology.Conventional(QueueType.Quorum),
    connectionString: "host=localhost");

var routing = endpointConfiguration.UseTransport(rabbitMqTransport);
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
routing.RouteToEndpoint(typeof(MyCommand), "Samples.RabbitMQ.SimpleReceiver");
endpointConfiguration.EnableInstallers();

var endpointInstance = await Endpoint.Start(endpointConfiguration);
await SendMessages(endpointInstance);
await endpointInstance.Stop();

static async Task SendMessages(IMessageSession messageSession)
{
    Console.WriteLine("Press [c] to send a command, or [e] to publish an event. Press [Esc] to exit.");

    while (true)
    {
        var input = Console.ReadKey();
        Console.WriteLine();

        switch (input.Key)
        {
            case ConsoleKey.C:
                await messageSession.Send(new MyCommand());
                break;
            case ConsoleKey.E:
                await messageSession.Publish(new MyEvent());
                break;
            case ConsoleKey.Escape:
                return;
        }
    }
}
