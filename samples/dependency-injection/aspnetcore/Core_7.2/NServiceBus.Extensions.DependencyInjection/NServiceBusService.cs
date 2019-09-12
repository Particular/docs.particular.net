namespace NServiceBus.Extensions.DependencyInjection
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    class NServiceBusService : IHostedService
    {
        public NServiceBusService(IStartableEndpointWithExternallyManagedContainer startableEndpoint, IServiceProvider serviceProvider)
        {
            this.startableEndpoint = startableEndpoint;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            endpoint = await startableEndpoint.Start(new ServiceProviderAdapter(serviceProvider))
                .ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return endpoint.Stop();
        }

        IEndpointInstance endpoint;
        readonly IStartableEndpointWithExternallyManagedContainer startableEndpoint;
        readonly IServiceProvider serviceProvider;
    }
}