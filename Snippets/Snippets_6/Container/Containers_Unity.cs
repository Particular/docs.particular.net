namespace Snippets6.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            #region Unity

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseContainer<UnityBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region Unity_Existing

            EndpointConfiguration configuration = new EndpointConfiguration();
            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));

            #endregion
        }

    }
}