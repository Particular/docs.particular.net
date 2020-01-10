using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Extensions.Hosting;
using System.Threading.Tasks;

namespace NServiceBus

{
    public class ServiceCollectionEndpoint
    {
        public static IStartableServiceCollectionEndpoint Create<TContainerBuilder>(
            EndpointConfiguration endpointConfiguration,
            IServiceProviderFactory<TContainerBuilder> serviceProviderFactory,
            IServiceCollection serviceCollection = null)
        {
            if(serviceCollection == null)
            {
                serviceCollection = new ServiceCollection();
            }

            return new TbdAdapter<TContainerBuilder>(EndpointWithExternallyManagedContainer.Create(endpointConfiguration,
                new ServiceCollectionAdapter(serviceCollection)),
                serviceProviderFactory,
                serviceCollection);
        }
    }

    class TbdAdapter<TContainerBuilder> : IStartableServiceCollectionEndpoint
    {

        public TbdAdapter(IStartableEndpointWithExternallyManagedContainer startableEndpointWithExternallyManagedContainer,
            IServiceProviderFactory<TContainerBuilder> serviceProviderFactory,
            IServiceCollection serviceCollection)
        {
            this.startableEndpointWithExternallyManagedContainer = startableEndpointWithExternallyManagedContainer;
            this.serviceProviderFactory = serviceProviderFactory;
            this.serviceCollection = serviceCollection;
        }

        public Task<IEndpointInstance> Start()
        {
            var builder = serviceProviderFactory.CreateBuilder(serviceCollection);
            var x = serviceProviderFactory.CreateServiceProvider(builder);

            return startableEndpointWithExternallyManagedContainer.Start(new ServiceProviderAdapter(x));
        }

        IStartableEndpointWithExternallyManagedContainer startableEndpointWithExternallyManagedContainer;
        IServiceProviderFactory<TContainerBuilder> serviceProviderFactory;
        private readonly IServiceCollection serviceCollection;
    }

    public interface IStartableServiceCollectionEndpoint
    {
        Task<IEndpointInstance> Start();
    }
}
