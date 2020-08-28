// ReSharper disable SuggestVarOrType_SimpleTypes
namespace Core8.Container.Custom
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;

    public class ExternallyManaged
    {
        async Task Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ExternalPrepare

            IServiceCollection serviceCollection = new ServiceCollection();

            var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

            #endregion

            #region ExternalStart

            IServiceProvider builder = serviceCollection.BuildServiceProvider();

            var startedEndpoint = await startableEndpoint.Start(builder);

            #endregion
        }
    }
}