using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NServiceBus
{
    class NServiceBusHostedService : IHostedService
    {
        public NServiceBusHostedService(IStartableEndpointWithExternallyManagedContainer startableEndpoint,
            IServiceProvider serviceProvider, IServiceCollection parentServiceCollection, ServiceCollection childCollection,
            SessionProvider sessionProvider,
            string endpointName)
        {
            this.parentServiceCollection = parentServiceCollection;
            this.endpointName = endpointName;
            this.sessionProvider = sessionProvider;
            this.childCollection = childCollection;
            this.startableEndpoint = startableEndpoint;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            AddSourceServicesFrom(childCollection, parentServiceCollection, serviceProvider);

            childServiceProvider = childCollection.BuildServiceProvider();

            endpoint = await startableEndpoint.Start(new ServiceProviderAdapter(childServiceProvider))
                .ConfigureAwait(false);

            managementScope = sessionProvider.Manage(endpoint, endpointName);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await endpoint.Stop().ConfigureAwait(false);
            managementScope.Dispose();
            await childServiceProvider.DisposeAsync().ConfigureAwait(false);
        }

        private static void AddSourceServicesFrom(
            IServiceCollection targetCollection,
            IServiceCollection sourceCollection,
            IServiceProvider sourceProvider)
        {
            foreach (var desc in sourceCollection)
            {
                ServiceDescriptor newDesc;

                if (desc.Lifetime == ServiceLifetime.Singleton && desc.ImplementationInstance == null && !desc.ServiceType.IsGenericTypeDefinition && !desc.ImplementationType?.IsGenericTypeDefinition == true)
                {
                    //convert singletons whose instance isn't known to a redirect call to the source provider

                    newDesc = new ServiceDescriptor(
                        serviceType: desc.ServiceType,
                        factory: x => sourceProvider.GetService(desc.ServiceType),
                        lifetime: ServiceLifetime.Singleton);
                }
                else
                {
                    newDesc = desc;
                }

                targetCollection.Add(newDesc);
            }
        }

        IEndpointInstance endpoint;
        readonly IStartableEndpointWithExternallyManagedContainer startableEndpoint;
        readonly IServiceProvider serviceProvider;
        private ServiceCollection childCollection;
        private ServiceProvider childServiceProvider;
        private SessionProvider sessionProvider;
        private string endpointName;
        private IDisposable managementScope;
        private IServiceCollection parentServiceCollection;
    }
}