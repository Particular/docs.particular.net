namespace Snippets5.Container
{
    using Autofac;
    using NServiceBus;

    class Containers_Autofac
    {
        Containers_Autofac(BusConfiguration busConfiguration)
        {
            #region Autofac

            busConfiguration.UseContainer<AutofacBuilder>();

            #endregion
        }

        void Existing(BusConfiguration busConfiguration)
        {
            #region Autofac_Existing

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            busConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

            #endregion
        }

    }
}