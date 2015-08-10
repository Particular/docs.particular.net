namespace Snippets4.Container
{
    using Castle.Windsor;
    using NServiceBus;
    using NServiceBus.ObjectBuilder.CastleWindsor;
    using NServiceBus.ObjectBuilder.Common.Config;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            #region CastleWindsor

            Configure.With()
                .UsingContainer<WindsorObjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            IWindsorContainer windsorContainer = null;

            #region CastleWindsor_Existing

            Configure.With()
                .UsingContainer(new WindsorObjectBuilder(windsorContainer));

            #endregion
        }

    }
}