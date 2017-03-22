namespace ServiceFabricPersistence_1
{
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
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

        #region ServiceFabricPersistenceSagaWithCustomCollectionName
        [ServiceFabricSaga(CollectionName = "custom-collection-name")]
        public class CustomCollectionNameSaga : IHandleMessages<Message>
        {
            public Task Handle(Message message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }
        #endregion

        #region ServiceFabricPersistenceSagaWithCustomSagaDataName
        [ServiceFabricSaga(SagaDataName = "saga-data-name")]
        public class ExplicitSagaDataNameSaga : IHandleMessages<Message>
        {
            public Task Handle(Message message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }
        #endregion

        public class Message : IMessage
        {
        }
    }
}