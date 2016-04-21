namespace NHibernate_7
{
    using global::NHibernate.Cfg;
    using NServiceBus;
    using NServiceBus.Persistence;

    class ConfiguringNHibernateSqlAzure
    {
        ConfiguringNHibernateSqlAzure(EndpointConfiguration endpointConfiguration)
        {
            #region SqlAzureNHibernateDriverConfiguration

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["connection.driver_class"] = "NHibernate.SqlAzure.SqlAzureClientDriver, NHibernate.SqlAzure"
                }
            };

            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.UseConfiguration(nhConfiguration);

            #endregion
        }

    }
}