namespace Snippets3.Container
{
    using Autofac;
    using NServiceBus;

    public class Containers_Autofac
    {
        public void Simple()
        {
            #region Autofac

            Configure configure = Configure.With();
            configure.AutofacBuilder();

            #endregion
        }

        public void Existing()
        {
            #region Autofac_Existing

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            Configure configure = Configure.With();
            configure.AutofacBuilder(container);

            #endregion
        }

    }
}