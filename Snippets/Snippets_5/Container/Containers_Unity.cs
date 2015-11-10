namespace Snippets5.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    public class Containers_Unity
    {
        public void Simple()
        {
            #region Unity

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<UnityBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region Unity_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            busConfiguration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));

            #endregion
        }

    }
}