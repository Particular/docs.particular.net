using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Serializers.Json;
using NServiceBus.Serializers.XML;

#region serialization-mapper
public class SerializationMapper
{
    JsonMessageSerializer jsonSerializer;
    XmlMessageSerializer xmlSerializer;

    public SerializationMapper(IMessageMapper mapper, Conventions conventions, Configure configure)
    {
        jsonSerializer = new JsonMessageSerializer(mapper);
        xmlSerializer = new XmlMessageSerializer(mapper, conventions);
        List<Type> messageTypes = configure.TypesToScan.Where(conventions.IsMessageType).ToList();
        xmlSerializer.Initialize(messageTypes);
    }

    public IMessageSerializer GetSerializer(Dictionary<string, string> headers)
    {
        string contentType;
        if (!headers.TryGetValue(Headers.ContentType, out contentType))
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
        var message = $"Could not derive serializer for contentType='{contentType}'";
        throw new Exception(message);
    }

    public IMessageSerializer GetSerializer(Type messageType)
    {
        var isJsonMessage = messageType.ContainsAttribute<SerializeWithJsonAttribute>();
        var isXmlMessage = messageType.ContainsAttribute<SerializeWithXmlAttribute>();
        if (isXmlMessage && isJsonMessage)
        {
            var message = $"Choose either [SerializeWithXml] or [SerializeWithJson] for serialization of '{messageType.Name}'.";
            throw new Exception(message);
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