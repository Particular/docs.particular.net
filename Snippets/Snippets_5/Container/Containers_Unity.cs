namespace Snippets5.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Unity

            busConfiguration.UseContainer<UnityBuilder>();

            #endregion
        }

        public void Existing()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Unity_Existing

            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            busConfiguration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));

            #endregion
        }

    }
}