namespace Snippets3.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringNHibernate

            Configure configure = Configure.With();
            configure.NHibernateSagaPersister();
            configure.UseNHibernateTimeoutPersister();
            configure.DBSubcriptionStorage();

            #endregion
        }

        public void SpecificNHibernateConfiguration()
        {
            #region SpecificNHibernateConfiguration

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

            Configure configure = Configure.With();
            configure.DBSubcriptionStorage(nhConfiguration, true);
            configure.UseNHibernateTimeoutPersister(nhConfiguration, true);
            // custom code nh configuration for sagas and gateways was not supported in version 3.;

            #endregion

        }
    }
}