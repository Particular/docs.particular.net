namespace Snippets3.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple()
        {
            Configure configure = Configure.With();
            #region Autofac

            configure.AutofacBuilder();

            #endregion
        }

        public void Existing()
        {
            Configure configure = Configure.With();
            #region Autofac_Existing

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            configure.AutofacBuilder(container);

            #endregion
        }

    }
}