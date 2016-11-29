using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceUsage

        endpointConfiguration.UsePersistence<SqlPersistence>();

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
        persistence.JsonSettings(settings);

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
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence, StorageType.Sagas>();
        persistence.JsonSettings(currentSettings);
        persistence.JsonSettingsForVersion((type, version) =>
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
        persistence.ReaderCreator(
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
        persistence.WriterCreator(
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