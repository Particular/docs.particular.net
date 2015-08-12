namespace Snippets5.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple()
        {
            #region Autofac

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<AutofacBuilder>();

            #endregion
        }

        public void Existing()
        {
            ILifetimeScope lifetimeScope = null;

            #region Autofac_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(lifetimeScope));

            #endregion
        }

    }
}