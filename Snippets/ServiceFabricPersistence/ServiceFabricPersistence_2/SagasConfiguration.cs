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

            var sagaSettings = persistence.SagaSettings();
            sagaSettings.JsonSettings(new JsonSerializerSettings
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

            var sagaSettings = persistence.SagaSettings();
            sagaSettings.ReaderCreator(
                readerCreator: textReader =>
                {
                    return new JsonTextReader(textReader);
                });
            #endregion
        }

        public void ConfigureCustomJsonWriter(EndpointConfiguration endpointConfiguration)
        {
            #region ServiceFabricPersistenceSagaWriterCreator

            var persistence = endpointConfiguration.UsePersistence<ServiceFabricPersistence>();

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

        #region ServiceFabricPersistenceSagaWithCustomCollectionName
        [ServiceFabricSaga(CollectionName = "custom-collection-name")]
        public class CustomCollectionNameSaga :
            Saga<CustomCollectionNameSaga.SagaData>,
            IHandleMessages<Message>
        {
            public Task Handle(Message message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }
        #endregion

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
            {
            }

            public class SagaData : ContainSagaData
            {
            }
        }

        #region ServiceFabricPersistenceSagaWithCustomSagaDataName
        [ServiceFabricSaga(SagaDataName = "saga-data-name")]
        public class ExplicitSagaDataNameSaga :
            Saga<ExplicitSagaDataNameSaga.SagaData>,
            IHandleMessages<Message>
        {
            public Task Handle(Message message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }

        #endregion

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
            {
            }

            public class SagaData : ContainSagaData
            {
            }
        }

        public class Message : IMessage
        {
        }
    }
}