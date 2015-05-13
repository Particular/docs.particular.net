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

    public void SpecificNHibernateConfiguration()
    {
        #region SpecificNHibernateConfiguration

        Configuration nhConfiguration = new Configuration();
        nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

        Configure.With()
            .UseNHibernateSubscriptionPersister(nhConfiguration)
            .UseNHibernateTimeoutPersister(nhConfiguration, true)
            .UseNHibernateSagaPersister(nhConfiguration)
            .UseNHibernateGatewayPersister(nhConfiguration);

        #endregion

    }
}