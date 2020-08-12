using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace Receiver
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                })
                .ConfigureServices(services => services.AddSingleton<IHostedService>(new ProceedIfRabbitMqIsAlive("rabbitmq")))
                .UseNServiceBus(ctx =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");
                    #region TransportConfiguration

                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.ConnectionString("host=rabbitmq");
                    transport.UseConventionalRoutingTopology();

                    #endregion

                    endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                    endpointConfiguration.EnableInstallers();
                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
                    return endpointConfiguration;
                });
        }

        static async Task OnCriticalError(ICriticalErrorContext context)
        {
            // TODO: decide if stopping the endpoint and exiting the process is the best response to a critical error
            // https://docs.particular.net/nservicebus/hosting/critical-errors
            try
            {
                await context.Stop();
            }
            finally
            {
                FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }

        static void FailFast(string message, Exception exception)
        {
            try
            {
                log.Fatal(message, exception);

                // TODO: when using an external logging framework it is important to flush any pending entries prior to calling FailFast
                // https://docs.particular.net/nservicebus/hosting/critical-errors#when-to-override-the-default-critical-error-action
            }
            finally
            {
                Environment.FailFast(message, exception);
            }
        }

        static readonly ILog log = LogManager.GetLogger(typeof(Program));
    }
}