namespace Snippets6.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            #region Unity

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseContainer<UnityBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region Unity_Existing

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            endpointConfiguration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));

            #endregion
        }

    }
}