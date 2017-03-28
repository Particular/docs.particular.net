using NServiceBus;
using NServiceBus.Persistence.Sql;

class Upgrade1to2
{

    void Schema(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2_Schema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.TablePrefix("MySchema.");

        #endregion
    }

    void SchemaExtended(EndpointConfiguration endpointConfiguration)
    {
        #region 1to2_Schema_Extended

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.TablePrefix("[My Schema].");

        #endregion
    }
}