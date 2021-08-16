using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System.Threading.Tasks;

namespace Extensions.DependencyInjection_1
{
    class Usage
    {
        void ConfigureServiceCollection(EndpointConfiguration endpointConfiguration)
        {
            #region usecontainer-servicecollection

            endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            #endregion
        }

        async Task ExternallyManagedMode(EndpointConfiguration endpointConfiguration)
        {
            #region externally-managed-mode

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<MyService>();

            var startableEndpoint = EndpointWithExternallyManagedServiceProvider.Create(endpointConfiguration, serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var endpoint = await startableEndpoint.Start(serviceProvider);

            serviceProvider.GetService<MyService>();

            #endregion
        }
    }

    class MyService
    {
    }
}