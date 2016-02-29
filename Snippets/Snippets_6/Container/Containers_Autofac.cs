namespace Snippets6.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple()
        {
            #region Autofac

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseContainer<AutofacBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region Autofac_Existing

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            endpointConfiguration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

            #endregion
        }

    }
}