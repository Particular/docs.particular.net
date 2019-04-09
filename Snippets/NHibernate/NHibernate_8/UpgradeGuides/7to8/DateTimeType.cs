using NHibernate.Cfg;
using NServiceBus;
using NServiceBus.Persistence;

class DateTimeType
{
    public void ForceLegacyDateTimeType(EndpointConfiguration endpointConfiguration)
    {
        #region NHibernateLegacyDateTimeUpgrade7To8

        var nhConfiguration = new Configuration
        {
            Properties =
            {
                [Environment.SqlTypesKeepDateTime] = bool.TrueString
            }
        };

        var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
        persistence.UseConfiguration(nhConfiguration);

        #endregion
    }
}