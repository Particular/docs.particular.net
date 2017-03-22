namespace ServiceFabricPersistence_1
{
    using System.Globalization;
    using System.IO;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using NServiceBus;
    using NServiceBus.Persistence.ServiceFabric;

    public class SagasConfiguration
    {
        public void ConfigureCustomJsonSerializerSettings(EndpointConfiguration endpointConfiguration)
        {
            #region ServiceFabricPersistenceSagaJsonSerializerSettings

            var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();

            persistence.SagaSettings().JsonSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters =
                {
                    new IsoDateTimeConverter
                    {
                        DateTimeStyles = DateTimeStyles.RoundtripKind
                    }
                }
            });

            #endregion
        }

        public void ConfigureCustomJsonReader(EndpointConfiguration endpointConfiguration)
        {
            #region ServiceFabricPersistenceSagaReaderCreator

            var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();

            persistence.SagaSettings().ReaderCreator(textReader => { return new JsonTextReader(textReader); });

            #endregion
        }

        public void ConfigureCustomJsonWriter(EndpointConfiguration endpointConfiguration)
        {
            #region ServiceFabricPersistenceSagaWriterCreator

            var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();

            persistence.SagaSettings().WriterCreator(builder =>
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
}