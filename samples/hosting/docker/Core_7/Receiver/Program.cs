using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

        Console.Title = "Samples.Docker.Receiver";

        var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");
        #region TransportConfiguration
        endpointConfiguration.UseTransport<RabbitMQTransport>()
            .ConnectionString("host=rabbitmq")
            .UseConventionalRoutingTopology()
            .DelayedDelivery().DisableTimeoutManager();
        #endregion
        endpointConfiguration.EnableInstallers();

        // The RabbitMQ container starts before endpoints but it may
        // take several seconds for the broker to become reachable.
        #region WaitForRabbitBeforeStart
        await WaitForRabbitToStart()
            .ConfigureAwait(false);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #endregion

        Console.WriteLine("Use docker-compose down to stop containers.");

        // Wait until the message arrives.
        closingEvent.WaitOne();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    #region WaitForRabbit
    static async Task WaitForRabbitToStart()
    {
        var factory = new ConnectionFactory
        {
            Uri = "amqp://rabbitmq"
        };
        for (var i = 0; i < 5; i++)
        {
            try
            {
                using (factory.CreateConnection())
                {
                }
                return;
            }
            catch (BrokerUnreachableException)
            {
            }
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
    #endregion

    static void OnExit(object sender, ConsoleCancelEventArgs args)
    {
        closingEvent.Set();
    }

    static AutoResetEvent closingEvent = new AutoResetEvent(false);
}
