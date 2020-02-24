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

        void ConfigureThirdPartyContainer(EndpointConfiguration endpointConfiguration)
        {
            #region usecontainer-thirdparty

            endpointConfiguration.UseContainer(new AutofacServiceProviderFactory());

            #endregion
        }

        async Task ExternallyManagedMode(EndpointConfiguration endpointConfiguration)
        {
            #region externally-managed-mode
            var serviceCollection = new ServiceCollection();

            var startableEndpoint = EndpointWithExternallyManagedServiceProvider.Create(endpointConfiguration, serviceCollection);

            var endpoint = await startableEndpoint.Start(serviceCollection.BuildServiceProvider());
            #endregion
        }
    }
}