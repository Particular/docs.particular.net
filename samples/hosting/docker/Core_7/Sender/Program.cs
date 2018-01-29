using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.CancelKeyPress += OnExit;

        Console.Title = "Samples.Docker.Sender";

        var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=rabbitmq");
        transport.UseConventionalRoutingTopology();
        endpointConfiguration.EnableInstallers();

        // The RabbitMQ container starts before endpoints but it may
        // take several seconds for the broker to become reachable.
        await RabbitHelper.WaitForRabbitToStart()
            .ConfigureAwait(false);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
                    .ConfigureAwait(false);

        Console.WriteLine("Sending a message...");

        var guid = Guid.NewGuid();
        Console.WriteLine($"Requesting to get data by id: {guid:N}");

        var message = new RequestMessage
        {
            Id = guid,
            Data = "String property value"
        };

        await endpointInstance.Send("Samples.Docker.Receiver", message)
            .ConfigureAwait(false);

        Console.WriteLine("Message sent.");
        Console.WriteLine("Use 'docker-compose down' to stop containers.");

        // Wait until the message arrives.
        closingEvent.WaitOne();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static void OnExit(object sender, ConsoleCancelEventArgs args)
    {
        closingEvent.Set();
    }

    static AutoResetEvent closingEvent = new AutoResetEvent(false);
}