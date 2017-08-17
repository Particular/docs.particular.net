using NServiceBus;
using NServiceBus.Persistence.Sql;

class Upgrade2to3
{
    void VariantToDialect(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3_VariantToDialect

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MySql);

        #endregion
    }

    void Schema(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3_Schema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.Schema("MySchema");

        #endregion
    }
}