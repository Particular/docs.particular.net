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
            #region Autofac_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

            #endregion
        }

    }
}