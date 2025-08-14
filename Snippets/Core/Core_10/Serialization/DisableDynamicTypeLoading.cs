using NServiceBus;

namespace Core.Serialization;

public class DisableDynamicTypeLoading
{
    public void Snippet(EndpointConfiguration endpointConfiguration)
    {
        #region disable-dynamic-type-loading

        var serializerSettings = endpointConfiguration.UseSerialization<XmlSerializer>();
        serializerSettings.DisableDynamicTypeLoading();

        #endregion
    }
}