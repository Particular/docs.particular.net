namespace Core9.DataBus.Custom
{
    using System;
    using System.IO;
    using NServiceBus;
    using NServiceBus.DataBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
}