using NServiceBus;
using NServiceBus.Persistence;

#region OutboxShareNHibernateConfiguration
public class ShareNHibernateConfiguration : INeedInitialization
{
    public void Customize(ConfigurationBuilder builder)
    {
        var configuration = BuildMyBusinessDataNHibernateConfiguration();

        builder.UsePersistence<NServiceBus.NHibernate>();
            .UseConfiguration(configuration);
    }

    NHibernate.Cfg.Configuration BuildMyBusinessDataNHibernateConfiguration()
    {
        // build and return your NHibernate Configuration here
        return null;
    }
}
#endregion

