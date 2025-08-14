namespace Core.Serialization;

using NServiceBus;

public class DisableContentTypeInference
{
    public void DisableInference(EndpointConfiguration endpointConfiguration)
    {
        #region disable-message-type-inference

        var serializerSettings = endpointConfiguration.UseSerialization<XmlSerializer>();
        serializerSettings.DisableMessageTypeInference();

        #endregion
    }
}