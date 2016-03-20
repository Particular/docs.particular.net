namespace Snippets3.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;

    class Usage
    {
        void ConfiguringNHibernate(Configure configure )
        {
            #region ConfiguringNHibernate

            configure.NHibernateSagaPersister();
            configure.UseNHibernateTimeoutPersister();
            configure.DBSubcriptionStorage();

            #endregion
        }

        void SpecificNHibernateConfiguration(Configure configure)
        {
            #region SpecificNHibernateConfiguration

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

            configure.DBSubcriptionStorage(nhConfiguration, true);
            configure.UseNHibernateTimeoutPersister(nhConfiguration, true);
            // custom code nh configuration for sagas and gateways was not supported in version 3.;

            #endregion

        }
    }
}