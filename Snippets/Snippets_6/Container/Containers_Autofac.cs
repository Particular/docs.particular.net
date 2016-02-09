namespace Snippets6.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple()
        {
            #region Autofac

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseContainer<AutofacBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region Autofac_Existing

            EndpointConfiguration configuration = new EndpointConfiguration();
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(container));

            #endregion
        }

    }
}