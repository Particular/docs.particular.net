using NServiceBus;
using NServiceBus.Persistence.Sql;

class Upgrade2to3
{
    void VariantToDialect(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3_VariantToDialect

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MySql>();

        #endregion
    }

    void DialectReturn(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3_DialectSettings

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var mySqlDialectSettings = persistence.SqlDialect<SqlDialect.MySql>();

        #endregion
    }

    void Schema(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3_Schema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("MySchema");

        #endregion
    }
}