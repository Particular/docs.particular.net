using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNServiceBus(this IServiceCollection services, EndpointConfiguration configuration)
    {
        var configuredEndpoint = Endpoint.Configure(configuration, new ServiceCollectionAdapter(services));

        services.AddSingleton(_ => configuredEndpoint.MessageSession.Value);
        services.AddSingleton<IHostedService>(serviceProvider => new NServiceBusService(configuredEndpoint, serviceProvider));

        return services;
    }
}