namespace ClaimCheck_2.ClaimCheck.Custom;

using System;
using System.IO;
using NServiceBus;
using NServiceBus.ClaimCheck;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region PluginCustomDataBus

        endpointConfiguration.UseClaimCheck(svc => new CustomClaimCheck(), new SystemJsonClaimCheckSerializer());

        #endregion

        #region SpecifyingSerializer

        endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();

        #endregion

        #region SpecifyingDeserializer

        endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>()
            .AddDeserializer<BsonClaimCheckSerializer>();

        #endregion
    }

    class BsonClaimCheckSerializer : IClaimCheckSerializer
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