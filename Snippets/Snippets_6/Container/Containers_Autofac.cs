namespace Snippets6.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Autofac

            endpointConfiguration.UseContainer<AutofacBuilder>();

            #endregion
        }

        public void Existing(EndpointConfiguration endpointConfiguration)
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