using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Settings;

#region serialization-mapper
public class SerializationMapper
{
    IMessageSerializer jsonSerializer;
    IMessageSerializer xmlSerializer;

    public SerializationMapper(IMessageMapper mapper, ReadOnlySettings settings)
    {
        xmlSerializer = new XmlSerializer()
            .Configure(settings)(mapper);
        jsonSerializer = new NewtonsoftSerializer()
            .Configure(settings)(mapper);
    }

    public IMessageSerializer GetSerializer(Dictionary<string, string> headers)
    {
        if (!headers.TryGetValue(Headers.ContentType, out var contentType))
        {
            // default to Json
            return jsonSerializer;
        }
        if (contentType == jsonSerializer.ContentType)
        {
            return jsonSerializer;
        }
        if (contentType == xmlSerializer.ContentType)
        {
            return xmlSerializer;
        }
        throw new Exception($"Could not derive serializer for contentType='{contentType}'");
    }

    public IMessageSerializer GetSerializer(Type messageType)
    {
        var isJsonMessage = messageType.ContainsAttribute<SerializeWithJsonAttribute>();
        var isXmlMessage = messageType.ContainsAttribute<SerializeWithXmlAttribute>();
        if (isXmlMessage && isJsonMessage)
        {
            throw new Exception($"Choose either [SerializeWithXml] or [SerializeWithJson] for serialization of '{messageType.Name}'.");
        }
        if (isXmlMessage)
        {
            return xmlSerializer;
        }
        // default to json
        return jsonSerializer;
    }
}
#endregion