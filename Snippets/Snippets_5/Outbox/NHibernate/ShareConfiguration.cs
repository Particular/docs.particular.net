using NServiceBus;
using NServiceBus.Persistence;

#region OutboxShareNHibernateConfiguration
public class ShareNHibernateConfiguration : INeedInitialization
{
    public void Customize(BusConfiguration configuration)
    {
        var hibernateConfiguration = BuildMyBusinessDataNHibernateConfiguration();

        configuration.UsePersistence<NHibernatePersistence>()
          .UseConfiguration(hibernateConfiguration);
    }

    NHibernate.Cfg.Configuration BuildMyBusinessDataNHibernateConfiguration()
    {
        // build and return your NHibernate Configuration here
        return null;
    }
}
#endregion

