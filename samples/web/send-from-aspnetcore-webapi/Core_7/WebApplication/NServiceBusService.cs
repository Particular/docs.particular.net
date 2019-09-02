using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

//TODO: Switch asp.net core 3.0 to avoid the race condition on start
class NServiceBusService : IHostedService
{
    public NServiceBusService(PreparedEndpoint preparedEndpoint, IServiceProvider serviceProvider)
    {
        this.preparedEndpoint = preparedEndpoint;
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        endpoint = await Endpoint.Start(preparedEndpoint, new ServiceProviderAdapter(serviceProvider))
            .ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return endpoint.Stop();
    }

    IEndpointInstance endpoint;
    readonly PreparedEndpoint preparedEndpoint;
    readonly IServiceProvider serviceProvider;
}