namespace Snippets3.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringNHibernate

            Configure.With()
                .DefaultBuilder()
                .NHibernateSagaPersister()
                .UseNHibernateTimeoutPersister()
                .DBSubcriptionStorage();

            #endregion
        }
        public void SpecificNHibernateConfiguration()
        {
            #region SpecificNHibernateConfiguration

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

            Configure.With()
                .DBSubcriptionStorage(nhConfiguration,true)
                .UseNHibernateTimeoutPersister(nhConfiguration, true);
            // custom code nh configuration for sagas and gateways was not supported in version 3.;

            #endregion

        }
    }
}