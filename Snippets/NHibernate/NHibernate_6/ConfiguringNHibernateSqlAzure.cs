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

        var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }

}