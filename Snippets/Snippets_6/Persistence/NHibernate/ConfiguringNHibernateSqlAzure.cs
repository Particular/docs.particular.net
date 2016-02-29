namespace Snippets6.Persistence.NHibernate
{
    using global::NHibernate.Cfg;
    using NServiceBus;
    using NServiceBus.Persistence;

    class ConfiguringNHibernateSqlAzure
    {
        public void SqlAzureNHibernateDriverConfiguration()
        {
            #region SqlAzureNHibernateDriverConfiguration

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

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