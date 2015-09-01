namespace Snippets4.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringNHibernate

            Configure configure = Configure.With();
            configure.UseNHibernateSubscriptionPersister();
            configure.UseNHibernateTimeoutPersister();
            configure.UseNHibernateSagaPersister();
            configure.UseNHibernateGatewayPersister();

            #endregion
        }

        public void SpecificNHibernateConfiguration()
        {
            #region SpecificNHibernateConfiguration

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["dialect"] = "NHibernate.Dialect.MsSql2008Dialect";

            Configure configure = Configure.With();
            configure.UseNHibernateSubscriptionPersister(nhConfiguration);
            configure.UseNHibernateTimeoutPersister(nhConfiguration, true);
            configure.UseNHibernateSagaPersister(nhConfiguration);
            configure.UseNHibernateGatewayPersister(nhConfiguration);

            #endregion

        }
    }
}