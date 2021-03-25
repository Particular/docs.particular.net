using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql;
using NpgsqlTypes;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Usage
{
    void SqlServerUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageSqlServer

        var connection = @"Data Source=.\SqlExpress;Initial Catalog=dbname;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
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
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        persistence.SqlDialect<SqlDialect.MySql>();
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new MySqlConnection(connection);
            });

        #endregion
    }

    void PostgreSqlUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsagePostgreSql

        var connection = "Server=localhost;Port=5432;Database=dbname;User Id=user;Password=pass;";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        dialect.JsonBParameterModifier(
            modifier: parameter =>
            {
                var npgsqlParameter = (NpgsqlParameter)parameter;
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
            });
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new NpgsqlConnection(connection);
            });

        #endregion
    }
    void JsonBParameterModifier(EndpointConfiguration endpointConfiguration)
    {
        #region JsonBParameterModifier

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        dialect.JsonBParameterModifier(
            modifier: parameter =>
            {
                var npgsqlParameter = (NpgsqlParameter)parameter;
                npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
            });

        #endregion
    }

    void OracleUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageOracle

        var connection = "Data Source=localhost;User Id=username;Password=pass;Enlist=false;";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));
        persistence.SqlDialect<SqlDialect.Oracle>();
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

    void PostgresTypeNameHandling(EndpointConfiguration endpointConfiguration)
    {
        #region PostgresTypeNameHandling

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
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

    void MsSqlSchema(EndpointConfiguration endpointConfiguration)
    {
        #region MsSqlSchema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("MySchema");

        #endregion
    }

    void OracleSchema(EndpointConfiguration endpointConfiguration)
    {
        #region OracleSchema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.Oracle>();
        dialect.Schema("MySchema");

        #endregion
    }

    void PostgreSqlSchema(EndpointConfiguration endpointConfiguration)
    {
        #region PostgreSqlSchema

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.PostgreSql>();
        dialect.Schema("MySchema");

        #endregion
    }

    void MsSqlDoNotShareConnection(EndpointConfiguration endpointConfiguration)
    {
        #region MsSqlDoNotShareConnection

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.DoNotUseSqlServerTransportConnection();

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

        #region ExecuteScriptsPostgreSql

        using (var connection = new NpgsqlConnection("ConnectionString"))
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

    async Task ScriptRunnerUsage()
    {
        const string connectionString = "";

        #region ScriptRunner

        await ScriptRunner.Install(
            sqlDialect: new SqlDialect.MsSqlServer(),
            tablePrefix: "MyEndpoint",
            connectionBuilder: () => new SqlConnection(connectionString), 
            scriptDirectory: @"C:\Scripts",
            shouldInstallOutbox: true,
            shouldInstallSagas: true,
            shouldInstallSubscriptions: true,
            shouldInstallTimeouts: true);

        #endregion
    }
}