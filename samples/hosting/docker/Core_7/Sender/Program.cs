using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

        Console.Title = "Samples.Docker.Sender";
                
        var endpointInstance = await TryStart()
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
        Console.WriteLine("Press Ctrl+C to exit.");

        // Wait until the message arrives.
        closingEvent.WaitOne();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task<IEndpointInstance> TryStart()
    {
        int count = 0;

        while (count < 5)
        {
            ++count;

            try
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");
                endpointConfiguration.UseTransport<RabbitMQTransport>()
                    .ConnectionString("host=rabbitmq")
                    .UseConventionalRoutingTopology()
                    .DelayedDelivery().DisableTimeoutManager();
                endpointConfiguration.EnableInstallers();

                var endpointInstance = await Endpoint.Start(endpointConfiguration)
                            .ConfigureAwait(false);

                return endpointInstance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start endpoint: {ex}");
                await Task.Delay(1000);
            }
        }

        throw new Exception($"Unable to start endpoint after {count} attempts.");
    }

    static void OnExit(object sender, ConsoleCancelEventArgs args)
    {
        closingEvent.Set();
    }

    static AutoResetEvent closingEvent = new AutoResetEvent(false);
}
