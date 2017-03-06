using NServiceBus.Persistence.Sql;

[assembly: SqlPersistenceSettings(
    msSqlServerScripts: true,
    mySqlScripts: false)]