using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using NServiceBus.Serializers.Binary;
using NServiceBus.Serializers.Json;

#region serialization-mapper
public class SerializationMapper
{
    JsonMessageSerializer jsonSerializer;
    BinaryMessageSerializer binarySerializer;

    public SerializationMapper(IMessageMapper mapper)
    {
        jsonSerializer = new JsonMessageSerializer(mapper);
        binarySerializer = new BinaryMessageSerializer();
    }
    
    public IMessageSerializer GetSerializer(Dictionary<string, string> headers)
    {
        string contentType;
        if (!headers.TryGetValue(Headers.ContentType, out contentType))
        {
            //default to Json
            return jsonSerializer;
        }
        if (contentType == jsonSerializer.ContentType)
        {
            return jsonSerializer;
        }
        if (contentType == binarySerializer.ContentType)
        {
            return binarySerializer;
        }
        string message = string.Format("Could not derive serializer for contentType='{0}'", contentType);
        throw new Exception(message);
    }

    public IMessageSerializer GetSerializer(Type messageType)
    {
        bool isJsonMessage = messageType.ContainsAttribute<SerializeWithJsonAttribute>();
        bool isBinaryMessage = messageType.ContainsAttribute<SerializeWithBinaryAttribute>();
        if (isBinaryMessage && isJsonMessage)
        {
            string message = string.Format("Choose either [SerializeWithBinary] or [SerializeWithJson] for serialization of '{0}'.", messageType.Name);
            throw new Exception(message);
        }
        if (isBinaryMessage)
        {
            return binarySerializer;
        }
        //default to json
        return jsonSerializer;
    }
}
#endregion