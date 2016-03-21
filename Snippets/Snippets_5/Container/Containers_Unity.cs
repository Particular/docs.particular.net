namespace Snippets5.Container
{
    using Microsoft.Practices.Unity;
    using NServiceBus;

    class Containers_Unity
    {
        Containers_Unity(BusConfiguration busConfiguration)
        {
            #region Unity

            busConfiguration.UseContainer<UnityBuilder>();

            #endregion
        }

        void Existing(BusConfiguration busConfiguration)
        {
            #region Unity_Existing

            UnityContainer container = new UnityContainer();
            container.RegisterInstance(new MyService());
            busConfiguration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container));

            #endregion
        }

    }
}