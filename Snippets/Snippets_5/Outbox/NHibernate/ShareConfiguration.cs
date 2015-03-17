using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;

public class ShareNHibernateConfiguration
{
    public void Customize()
    {
        BusConfiguration configuration = null;

        #region OutboxShareNHibernateConfiguration

        Configuration hibernateConfiguration = BuildMyBusinessDataNHibernateConfiguration();

        configuration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfiguration);

        #endregion
    }

    Configuration BuildMyBusinessDataNHibernateConfiguration()
    {
        // build and return your NHibernate Configuration here
        return null;
    }
}

