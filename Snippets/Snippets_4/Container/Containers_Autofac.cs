namespace Snippets4.Container
{
    using Autofac;
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Autofac;
    using NServiceBus.ObjectBuilder.Common.Config;

    public class Containers_Autofac
    {
        public void Simple()
        {
            #region Autofac

            Configure.With()
                .UsingContainer<AutofacObjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            ILifetimeScope lifetimeScope = null;

            #region Autofac_Existing

            Configure.With()
                .UsingContainer(new AutofacObjectBuilder(lifetimeScope));

            #endregion
        }

    }
}