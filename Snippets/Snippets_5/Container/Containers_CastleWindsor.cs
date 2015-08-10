namespace Snippets5.Container
{
    using Castle.Windsor;
    using NServiceBus;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            #region CastleWindsor

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<WindsorBuilder>();

            #endregion
        }

        public void Existing()
        {
            IWindsorContainer windsorContainer = null;

            #region CastleWindsor_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(windsorContainer));

            #endregion
        }

    }
}