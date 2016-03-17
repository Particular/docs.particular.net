namespace Snippets5.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Autofac

            busConfiguration.UseContainer<AutofacBuilder>();

            #endregion
        }

        public void Existing()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Autofac_Existing

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

            #endregion
        }

    }
}