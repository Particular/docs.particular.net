namespace Snippets6.Container
{
    using Autofac;
    using NServiceBus;

    class Containers_Autofac
    {
        Containers_Autofac(EndpointConfiguration endpointConfiguration)
        {
            #region Autofac

            endpointConfiguration.UseContainer<AutofacBuilder>();

            #endregion
        }

        void Existing(EndpointConfiguration endpointConfiguration)
        {
            #region Autofac_Existing

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            endpointConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

            #endregion
        }

    }
}