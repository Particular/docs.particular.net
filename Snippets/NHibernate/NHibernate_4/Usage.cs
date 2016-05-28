using System;
using global::NHibernate.Cfg;
using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region ConfiguringNHibernate

        configure.UseNHibernateSubscriptionPersister();
        configure.UseNHibernateTimeoutPersister();
        configure.UseNHibernateSagaPersister();
        configure.UseNHibernateGatewayPersister();

        #endregion
    }

    void SpecificNHibernateConfiguration(Configure configure)
    {
        #region SpecificNHibernateConfiguration

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                ["dialect"] = "NHibernate.Dialect.MsSql2008Dialect"
            }
        };

        configure.UseNHibernateSubscriptionPersister(nhConfiguration);
        configure.UseNHibernateTimeoutPersister(nhConfiguration, true);
        configure.UseNHibernateSagaPersister(nhConfiguration);
        configure.UseNHibernateGatewayPersister(nhConfiguration);

        #endregion
    }

    void NHibernateSubscriptionCaching(Configure configure)
    {

        #region NHibernateSubscriptionCaching

        configure.UseNHibernateSubscriptionPersister(
            cacheExpiration: TimeSpan.FromSeconds(10));

        #endregion
    }
}