using NServiceBus;
using Unity;
using Unity.Microsoft.DependencyInjection;

class UnityUsage
{
    UnityUsage(EndpointConfiguration endpointConfiguration)
    {
        #region UnityUsage

        var container = new UnityContainer();
        container.RegisterInstance(new MyService());

        endpointConfiguration.UseContainer<IUnityContainer>(new ServiceProviderFactory(container));

        #endregion
    }

    class MyService
    {
    }
}