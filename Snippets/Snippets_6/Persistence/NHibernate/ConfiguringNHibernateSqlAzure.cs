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

            BusConfiguration busConfiguration = new BusConfiguration();

            Configuration nhConfiguration = new Configuration
            {
                Properties =
                {
                    ["connection.driver_class"] = "NHibernate.SqlAzure.SqlAzureClientDriver, NHibernate.SqlAzure"
                }
            };

            busConfiguration.UsePersistence<NHibernatePersistence>()
                .UseConfiguration(nhConfiguration);

            #endregion
        }

    }
}