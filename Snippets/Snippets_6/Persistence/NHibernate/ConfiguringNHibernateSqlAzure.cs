namespace Snippets6.Persistence.NHibernate
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

            endpointConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

    }
}