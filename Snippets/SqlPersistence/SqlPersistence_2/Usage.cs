using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;

class Usage
{
    void SqlServerUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageSqlServer

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var connection = @"Data Source=.\SqlExpress;Initial Catalog=dbname;Integrated Security=True";
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });

        #endregion
    }

    void DisableInstallers(EndpointConfiguration endpointConfiguration)
    {
        #region DisableInstaller

        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.DisableInstaller();

        #endregion
    }

    void ExecuteAtStartup(EndpointConfiguration endpointConfiguration)
    {
        #region ExecuteAtStartup

        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

        #endregion
    }

    void InstallerWorkflow(EndpointConfiguration endpointConfiguration)
    {
        #region InstallerWorkflow

        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var isDevelopment = Environment.GetEnvironmentVariable("IsDevelopment") == "true";
        if (!isDevelopment)
        {
            persistence.DisableInstaller();
        }

        #endregion
    }

    void MySqlUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageMySql

        var connection = "server=localhost;user=root;database=dbname;port=3306;password=pass;AllowUserVariables=True;AutoEnlist=false";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MySql);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new MySqlConnection(connection);
            });

        #endregion
    }

    void OracleUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageOracle

        var connection = "Data Source=localhost;User Id=username;Password=pass;Enlist=false;";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.Oracle);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new OracleConnection(connection);
            });

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceCustomSettings

        var settings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Include,
            Converters =
            {
                new IsoDateTimeConverter
                {
                    DateTimeStyles = DateTimeStyles.RoundtripKind
                }
            }
        };
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var sagaSettings = persistence.SagaSettings();
        sagaSettings.JsonSettings(settings);

        #endregion
    }

    void JsonSettingsForVersion(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceJsonSettingsForVersion

        var currentSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };
        var settingForVersion1 = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Sagas>();
        var sagaSettings = persistence.SagaSettings();
        sagaSettings.JsonSettings(currentSettings);
        sagaSettings.JsonSettingsForVersion(
            builder: (type, version) =>
            {
                if (version < new Version(2, 0))
                {
                    return settingForVersion1;
                }
                // default to what is defined by persistence.JsonSettings()
                return null;
            });

        #endregion
    }

    void CustomReader(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceCustomReader

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var sagaSettings = persistence.SagaSettings();
        sagaSettings.ReaderCreator(
            readerCreator: textReader =>
            {
                return new JsonTextReader(textReader);
            });

        #endregion
    }

    void CustomWriter(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceCustomWriter

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var sagaSettings = persistence.SagaSettings();
        sagaSettings.WriterCreator(
            writerCreator: builder =>
            {
                var writer = new StringWriter(builder);
                return new JsonTextWriter(writer)
                {
                    Formatting = Formatting.None
                };
            });

        #endregion
    }

    void TablePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region TablePrefix

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.TablePrefix("ThePrefix");

        #endregion
    }

    void OracleSchema(EndpointConfiguration endpointConfiguration)
    {
        #region OracleSchema 2.1

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.Oracle);
        persistence.Schema("MySchema");

        #endregion
    }

    void MsSqlSchema(EndpointConfiguration endpointConfiguration)
    {
        #region MsSqlSchema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MsSqlServer);
        persistence.Schema("MySchema");

        #endregion
    }

    void ExecuteScripts(string scriptDirectory, string tablePrefix)
    {
        #region ExecuteScriptsSqlServer

        using (var connection = new SqlConnection("ConnectionString"))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                foreach (var createScript in Directory.EnumerateFiles(
                    path: scriptDirectory,
                    searchPattern: "*_Create.sql",
                    searchOption: SearchOption.AllDirectories))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = File.ReadAllText(createScript);
                        var tablePrefixParameter = command.CreateParameter();
                        tablePrefixParameter.ParameterName = "tablePrefix";
                        tablePrefixParameter.Value = tablePrefix;
                        command.Parameters.Add(tablePrefixParameter);
                        var schemaParameter = command.CreateParameter();
                        schemaParameter.ParameterName = "schema";
                        schemaParameter.Value = "dbo";
                        command.Parameters.Add(schemaParameter);
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
        }

        #endregion

        #region ExecuteScriptsMySql

        using (var connection = new MySqlConnection("ConnectionString"))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                foreach (var createScript in Directory.EnumerateFiles(
                    path: scriptDirectory,
                    searchPattern: "*_Create.sql",
                    searchOption: SearchOption.AllDirectories))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = File.ReadAllText(createScript);
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "tablePrefix";
                        parameter.Value = tablePrefix;
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
        }

        #endregion

        #region ExecuteScriptsOracle

        using (var connection = new OracleConnection("ConnectionString"))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                foreach (var createScript in Directory.EnumerateFiles(
                    path: scriptDirectory,
                    searchPattern: "*_Create.sql",
                    searchOption: SearchOption.AllDirectories))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = File.ReadAllText(createScript);
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "tablePrefix";
                        parameter.Value = tablePrefix;
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
        }

        #endregion
    }
}