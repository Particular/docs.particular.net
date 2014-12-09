using NServiceBus;
using NServiceBus.Persistence;

public class ShareNHibernateConfiguration
{
    public void Customize()
    {
        BusConfiguration configuration = null;

        #region OutboxShareNHibernateConfiguration

        var hibernateConfiguration = BuildMyBusinessDataNHibernateConfiguration();

        configuration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfiguration);

        #endregion
    }

    NHibernate.Cfg.Configuration BuildMyBusinessDataNHibernateConfiguration()
    {
        // build and return your NHibernate Configuration here
        return null;
    }
}

