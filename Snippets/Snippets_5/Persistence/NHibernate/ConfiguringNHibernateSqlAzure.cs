namespace Snippets5.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;
    using NServiceBus.Persistence;

    class ConfiguringNHibernateSqlAzure
    {
        ConfiguringNHibernateSqlAzure(BusConfiguration busConfiguration)
        {
            #region SqlAzureNHibernateDriverConfiguration

            Configuration nhConfiguration = new Configuration();
            nhConfiguration.Properties["connection.driver_class"] = "NHibernate.SqlAzure.SqlAzureClientDriver, NHibernate.SqlAzure";

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

    }
}