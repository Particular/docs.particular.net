using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NServiceBus.Transport.AzureServiceBus;

public static class ServiceCollectionExtensions
{
    public static void AddAzureServiceBusTopology(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        #region OptionsValidation
        services.AddSingleton<IValidateOptions<TopologyOptions>, TopologyOptionsValidator>();
        services.AddOptions<TopologyOptions>().Bind(configuration.GetSection("AzureServiceBus:Topology")).ValidateOnStart();
        #endregion
    }
}