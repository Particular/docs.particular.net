namespace CustomDataBus
{
    using NServiceBus.ClaimCheck.DataBus;
    using NServiceBus.Features;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;
    using System.Threading.Tasks;
    using System.Threading;
    using System;

    #region CustomDataBusFeature
    class CustomDatabusFeature : Feature
    {
        public CustomDatabusFeature()
            => DependsOn<DataBus>();

        protected override void Setup(FeatureConfigurationContext context)
            => context.Services.AddSingleton<IDataBus, CustomDataBus>();
    }
    #endregion

    #region CustomDataBus

    class CustomDataBus :
        IDataBus
    {
        public Task<Stream> Get(string key, CancellationToken cancellationToken)
        {
            Stream stream = File.OpenRead("blob.dat");
            return Task.FromResult(stream);
        }

        public async Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken)
        {
            using (var destination = File.OpenWrite("blob.dat"))
            {
                await stream.CopyToAsync(destination, 81920, cancellationToken);
            }
            return "the-key-of-the-stored-file-such-as-the-full-path";
        }

        public Task Start(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    #endregion

    #region CustomDataBusDefinition
    class CustomDatabusDefinition : DataBusDefinition
    {
        protected override Type ProvidedByFeature()
            => typeof(CustomDatabusFeature);
    }
    #endregion

    class Usage
    {
        Usage(NServiceBus.EndpointConfiguration endpointConfiguration)
        {
            #region PluginCustomDataBus
            endpointConfiguration.UseDataBus(svc => new CustomDataBus(), new SystemJsonDataBusSerializer());
            #endregion

            #region SpecifyingSerializer
            endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            #endregion

            #region SpecifyingDeserializer
            endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>()
                .AddDeserializer<BsonDataBusSerializer>();
            #endregion
        }

        class BsonDataBusSerializer : IDataBusSerializer
        {
            public void Serialize(object databusProperty, Stream stream)
            {
            }

            public object Deserialize(Type propertyType, Stream stream)
            {
                return default;
            }

            public string ContentType { get; }
        }
    }
}