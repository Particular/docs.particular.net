using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;

namespace Templates_4
{
    class NServiceBusDockerContainer
    {
        static readonly ILog log = LogManager.GetLogger<NServiceBusDockerContainer>();

        #region DockerCreateHostBuilder
        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .UseNServiceBus(ctx =>
                {
                    // TODO: consider moving common endpoint configuration into a shared project
                    // for use by all endpoints in the system

                    // TODO: give the endpoint an appropriate name
                    var endpointConfiguration = new EndpointConfiguration("EndpointName");

                    // TODO: ensure the most appropriate serializer is chosen
                    // https://docs.particular.net/nservicebus/serialization/
                    endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                    // TODO: remove this condition after choosing a transport, persistence and deployment method suitable for production
                    if (Environment.UserInteractive && Debugger.IsAttached)
                    {
                        // TODO: choose a durable transport for production
                        // https://docs.particular.net/transports/
                        var transportExtensions = endpointConfiguration.UseTransport<LearningTransport>();

                        // TODO: choose a durable persistence for production
                        // https://docs.particular.net/persistence/
                        endpointConfiguration.UsePersistence<LearningPersistence>();

                        // TODO: create a script for deployment to production
                        endpointConfiguration.EnableInstallers();
                    }

                    // TODO: replace the license.xml file with a valid license file

                    return endpointConfiguration;
                });
        }
        #endregion

        #region DockerErrorHandling
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
        #endregion
    }
}
