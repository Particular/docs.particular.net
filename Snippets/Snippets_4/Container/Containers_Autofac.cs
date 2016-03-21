namespace Snippets4.Container
{
    using Autofac;
    using NServiceBus;

    class Containers_Autofac
    {
         void Simple(Configure configure)
        {
            #region Autofac

            configure.AutofacBuilder();

            #endregion
        }

        void Existing(Configure configure)
        {
            #region Autofac_Existing

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new MyService());
            IContainer container = builder.Build();
            configure.AutofacBuilder(container);

            #endregion
        }

    }
}