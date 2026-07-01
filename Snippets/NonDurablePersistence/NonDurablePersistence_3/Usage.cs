namespace NonDurablePersistence_3
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using NServiceBus;
    using NServiceBus.Persistence.NonDurable;

    class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurable

            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Sagas>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Subscriptions>();
            endpointConfiguration.UsePersistence<NonDurablePersistence, StorageType.Outbox>();

            #endregion
        }

        void ConfigureWithShortcut(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurableShortcut

            endpointConfiguration.UseNonDurablePersistence();

            #endregion
        }

        void ConfigureSharedStorage(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurableSharedStorage

            var sharedStorage = new NonDurableStorage();

            var options = new NonDurablePersistenceOptions
            {
                Storage = sharedStorage
            };

            endpointConfiguration.UseNonDurablePersistence(options);

            #endregion
        }

        void ConfigureWithOptions(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurableOptions

            var sharedStorage = new NonDurableStorage();

            var options = new NonDurablePersistenceOptions
            {
                Storage = sharedStorage,
                TimeProvider = System.TimeProvider.System,
                Saga = new NonDurableSagaOptions
                {
                    JsonSerializerOptions = new JsonSerializerOptions
                    {
                        TypeInfoResolverChain = { new SagaJsonContext() }
                    }
                }
            };

            endpointConfiguration.UseNonDurablePersistence(options);

            #endregion
        }

        void ConfigureSagaSerialization(EndpointConfiguration endpointConfiguration)
        {
            #region ConfiguringNonDurableSagaSerialization

            var options = new NonDurablePersistenceOptions
            {
                Saga = new NonDurableSagaOptions
                {
                    JsonSerializerOptions = new JsonSerializerOptions
                    {
                        TypeInfoResolverChain = { new SagaJsonContext() }
                    }
                }
            };

            endpointConfiguration.UseNonDurablePersistence(options);

            #endregion
        }
    }

    [JsonSerializable(typeof(MySagaData))]
    partial class SagaJsonContext : JsonSerializerContext
    {
    }

    class MySagaData : ContainSagaData
    {
        public string OrderId { get; set; } = string.Empty;
    }
}
