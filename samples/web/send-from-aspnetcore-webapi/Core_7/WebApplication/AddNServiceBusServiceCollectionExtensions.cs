using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

public static class AddNServiceBusServiceCollectionExtensions
{
    public static IServiceCollection AddNServiceBus(this IServiceCollection services, EndpointConfiguration configuration)
    {
        var preparedEndpoint = Endpoint.Prepare(configuration, new ServiceCollectionAdapter(services));

        services.AddSingleton(preparedEndpoint);
        services.AddSingleton(_ => preparedEndpoint.MessageSession);
        services.AddHostedService<NServiceBusService>();

        return services;
    }
}