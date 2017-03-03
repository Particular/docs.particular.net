using NServiceBus.Persistence.Sql;

#region SqlPersistenceSettings 1.1

[assembly: SqlPersistenceSettings(
    msSqlServerScripts: true,
    mySqlScripts: false)]

#endregion