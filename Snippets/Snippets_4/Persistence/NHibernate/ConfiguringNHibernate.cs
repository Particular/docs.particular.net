using NHibernate.Cfg;
using NServiceBus;

class ConfiguringNHibernate
{
    public void Foo()
    {
        #region ConfiguringNHibernate

        Configure.With()
        .UseNHibernateSubscriptionPersister()
        .UseNHibernateTimeoutPersister() 
        .UseNHibernateSagaPersister() 
        .UseNHibernateGatewayPersister();

        #endregion
    }

    public void CustomSpecificConfiguration()
    {
        #region CustomSpecificConfiguration

        Configuration config = new Configuration();
        config.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        Configure.With()
         .UseNHibernateSubscriptionPersister(config)
         .UseNHibernateTimeoutPersister(config, true)
         .UseNHibernateSagaPersister(config)
         .UseNHibernateGatewayPersister(config);

        #endregion

    }
}