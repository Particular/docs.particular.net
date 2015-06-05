namespace Snippets5.Outbox.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;
    using NServiceBus.Persistence;

    public class ShareConfiguration
    {
        public ShareConfiguration()
        {
            BusConfiguration busConfiguration = null;

            #region OutboxShareNHibernateConfiguration

            Configuration hibernateConfiguration = BuildMyBusinessDataNHibernateConfiguration();

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(hibernateConfiguration);

            #endregion
        }

        Configuration BuildMyBusinessDataNHibernateConfiguration()
        {
            // build and return your NHibernate Configuration here
            return null;
        }
    }

}

