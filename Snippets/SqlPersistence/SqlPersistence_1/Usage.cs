using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
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

        var connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=sqlpersistencesample;Integrated Security=True";
        persistence.SqlVarient(SqlVarient.MsSqlServer);
        persistence.TablePrefix("AcceptanceTests");
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connectionString);
            });

        #endregion
    }

    void MySqlUsage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsageMySql

        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var connectionString = "server=localhost;user=root;database=sqlpersistencesample;port=3306;password=Password1;Allow User Variables=True";
        persistence.SqlVarient(SqlVarient.MySql);
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new MySqlConnection(connectionString);
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

}