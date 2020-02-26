using NServiceBus;
using Unity;
using Unity.Microsoft.DependencyInjection;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Unity

        var container = new UnityContainer();
        container.RegisterInstance(new MyService());

        endpointConfiguration.UseContainer<IUnityContainer>(new ServiceProviderFactory(container));

        #endregion
    }

    class MyService
    {
    }
}