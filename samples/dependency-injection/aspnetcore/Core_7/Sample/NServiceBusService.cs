using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class NServiceBusService : IHostedService
{
    public NServiceBusService(IConfiguredEndpoint configuredEndpoint, IServiceProvider serviceProvider)
    {
        this.configuredEndpoint = configuredEndpoint;
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        endpoint = await configuredEndpoint.Start(new ServiceProviderAdapter(serviceProvider))
            .ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return endpoint.Stop();
    }

    IEndpointInstance endpoint;
    readonly IConfiguredEndpoint configuredEndpoint;
    readonly IServiceProvider serviceProvider;
}