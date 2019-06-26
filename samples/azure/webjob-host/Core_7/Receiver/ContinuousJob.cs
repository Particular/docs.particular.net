namespace Receiver
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Logging;

    public class ContinuousJob : IJobHost
    {
        public ContinuousJob(IConfiguration configuration, IServiceCollection services)
        {
            this.configuration = configuration;
            this.services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            endpointConfiguration = new EndpointConfiguration("receiver");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.EnableInstallers();

            var transportConnectionString = configuration.GetConnectionString("TransportConnectionString");

            var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
            transport.ConnectionString(transportConnectionString);

            endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

            endpointConfiguration.UseContainer<ServicesBuilder>(customizations => { customizations.ExistingServices(services); });

            endpoint = await Endpoint.Start(endpointConfiguration);
        }

        public async Task StopAsync()
        {
            await endpoint.Stop();
        }

        public Task CallAsync(string name, IDictionary<string, object> arguments = null, CancellationToken cancellationToken = new CancellationToken())
        {
            // we don't need it
            throw new NotImplementedException();
        }

        Task OnCriticalError(ICriticalErrorContext context)
        {
            var fatalMessage = $"The following critical error was encountered:\n{context.Error}\nProcess is shutting down.";
            Logger.Fatal(fatalMessage, context.Exception);

            Environment.FailFast(fatalMessage, context.Exception);

            return Task.CompletedTask;
        }

        readonly IConfiguration configuration;
        readonly IServiceCollection services;
        EndpointConfiguration endpointConfiguration;
        IEndpointInstance endpoint;
        static ILog Logger = LogManager.GetLogger<ContinuousJob>();
    }
}