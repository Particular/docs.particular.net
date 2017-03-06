using NServiceBus.Persistence.Sql;

#region SqlPersistenceSettings

[assembly: SqlPersistenceSettings(
    msSqlServerScripts: true,
    mySqlScripts: false)]

#endregion