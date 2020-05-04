namespace NServiceBus
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Configuration.AdvancedExtensibility;

    /// <summary>
    /// Extension methods to configure NServiceBus for the .NET Core generic host.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures the host to start an NServiceBus endpoint.
        /// </summary>
        public static IHostBuilder UseMultipleNServiceBus(this IHostBuilder hostBuilder, Func<HostBuilderContext, EndpointConfiguration> endpointConfigurationBuilder)
        {
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                var endpointConfiguration = endpointConfigurationBuilder(ctx);
                var endpointName = endpointConfiguration.GetSettings().Get<string>("NServiceBus.Routing.EndpointName");
                var childCollection = new ServiceCollection();
                var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, new ServiceCollectionAdapter(childCollection));

                if (!ctx.Properties.TryGetValue("NServiceBus.SessionProvider", out var provider))
                {
                    provider = new SessionProvider();
                    ctx.Properties.Add("NServiceBus.SessionProvider", provider);
                }

                var sessionProvider = (SessionProvider)provider;
                serviceCollection.AddSingleton<ISessionProvider>(sessionProvider);
                serviceCollection.AddSingleton<IHostedService>(serviceProvider => new NServiceBusHostedService(startableEndpoint, serviceProvider, serviceCollection, childCollection, sessionProvider, endpointName));
            });

            return hostBuilder;
        }
    }
}