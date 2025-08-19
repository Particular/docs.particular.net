using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;

namespace NHibernate;

class UsageSqlAzure
{
    UsageSqlAzure(EndpointConfiguration endpointConfiguration)
    {
        #region SqlAzureNHibernateDriverConfiguration

        var nhConfiguration = new Configuration
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