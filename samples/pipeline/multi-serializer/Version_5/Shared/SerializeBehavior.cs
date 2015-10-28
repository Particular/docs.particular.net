using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

#region serialize-behavior
class SerializeBehavior : IBehavior<OutgoingContext>
{
    SerializationMapper serializationMapper;

    public SerializeBehavior(SerializationMapper serializationMapper)
    {
        this.serializationMapper = serializationMapper;
    }

    public void Invoke(OutgoingContext context, Action next)
    {
        TransportMessage transportMessage = context.OutgoingMessage;
        if (!transportMessage.IsControlMessage())
        {
            LogicalMessage logicalMessage = context.OutgoingLogicalMessage;
            object messageInstance = logicalMessage.Instance;
            Type messageType = messageInstance.GetType();
            
            IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);
            transportMessage.Body = Serialize(messageSerializer, messageInstance);

            Dictionary<string, string> transportHeaders = transportMessage.Headers;
            transportHeaders[Headers.ContentType] = messageSerializer.ContentType;
            transportHeaders[Headers.EnclosedMessageTypes] = SerializeEnclosedMessageTypes(logicalMessage);

            foreach (KeyValuePair<string, string> headerEntry in logicalMessage.Headers)
            {
                transportHeaders[headerEntry.Key] = headerEntry.Value;
            }
        }

        next();
    }

    static byte[] Serialize(IMessageSerializer messageSerializer, object messageInstance)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            messageSerializer.Serialize(messageInstance, stream);
            return stream.ToArray();
        }
    }

    string SerializeEnclosedMessageTypes(LogicalMessage message)
    {
        IEnumerable<Type> distinctTypes = message.Metadata.MessageHierarchy.Distinct();

        return string.Join(";", distinctTypes.Select(t => t.AssemblyQualifiedName));
    }

}
#endregion