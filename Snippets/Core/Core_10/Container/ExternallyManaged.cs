
namespace Core.Container;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

public class ExternallyManaged
{
    async Task Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region ExternalPrepare

        IServiceCollection serviceCollection = new ServiceCollection();

        var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, serviceCollection);

        #endregion

        #region ExternalStart

        IServiceProvider builder = serviceCollection.BuildServiceProvider();

        var startedEndpoint = await startableEndpoint.Start(builder);

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }
}