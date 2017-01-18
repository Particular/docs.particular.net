using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
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

        var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=sqlpersistencesample;Integrated Security=True";
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


    void InstallerWorkflow(EndpointConfiguration endpointConfiguration)
    {
        #region InstallerWorkflow

        endpointConfiguration.EnableInstallers();
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var isDevelopement = Environment.GetEnvironmentVariable("IsDevelopement") == "true";
        if (!isDevelopement)
        {
            persistence.DisableInstaller();
        }

        #endregion
    }

    void MySqlUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageMySql

        var connection = "server=localhost;user=root;database=sqlpersistencesample;port=3306;password=Password1;AllowUserVariables=True;AutoEnlist=false";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlVariant(SqlVariant.MySql);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new MySqlConnection(connection);
            });

        #endregion
    }


    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceCustomSettings

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
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

    void SagaTablePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region SagaTablePrefix

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Sagas>();
        persistence.TablePrefix("TheSagaPrefix");

        #endregion
    }

    void TimeoutTablePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region TimeoutTablePrefix

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Timeouts>();
        persistence.TablePrefix("TheTimeoutPrefix");

        #endregion
    }

    void SubscriptionTablePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region SubscriptionTablePrefix

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Subscriptions>();
        persistence.TablePrefix("TheSubscriptionPrefix");

        #endregion
    }

    void OutboxTablePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region OutboxTablePrefix

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Outbox>();
        persistence.TablePrefix("TheOutboxPrefix");

        #endregion
    }

    void TablePrefix(EndpointConfiguration endpointConfiguration)
    {
        #region TablePrefix

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.TablePrefix("ThePrefixForAllStorages");

        #endregion
    }

    async Task ExecuteScripts(string scriptDirectory, string tablePrefix)
    {
        #region ExecuteScripts

        using (var connection = new SqlConnection("ConnectionString"))
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);
            foreach (var createScript in Directory.EnumerateFiles(
                path: scriptDirectory,
                searchPattern: "*_Create.sql",
                searchOption: SearchOption.AllDirectories))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = File.ReadAllText(createScript);
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "tablePrefix";
                    parameter.Value = tablePrefix;
                    command.Parameters.Add(parameter);
                    await command.ExecuteNonQueryAsync()
                        .ConfigureAwait(false);
                }
            }
        }

        #endregion
    }

}