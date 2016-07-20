using System;
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
        var transportMessage = context.OutgoingMessage;
        if (!transportMessage.IsControlMessage())
        {
            var logicalMessage = context.OutgoingLogicalMessage;
            var messageInstance = logicalMessage.Instance;
            var messageType = messageInstance.GetType();

            var messageSerializer = serializationMapper.GetSerializer(messageType);
            transportMessage.Body = Serialize(messageSerializer, messageInstance);

            var headers = transportMessage.Headers;
            headers[Headers.ContentType] = messageSerializer.ContentType;
            headers[Headers.EnclosedMessageTypes] = SerializeEnclosedMessageTypes(logicalMessage);

            foreach (var headerEntry in logicalMessage.Headers)
            {
                headers[headerEntry.Key] = headerEntry.Value;
            }
        }

        next();
    }

    static byte[] Serialize(IMessageSerializer messageSerializer, object messageInstance)
    {
        using (var stream = new MemoryStream())
        {
            messageSerializer.Serialize(messageInstance, stream);
            return stream.ToArray();
        }
    }

    string SerializeEnclosedMessageTypes(LogicalMessage message)
    {
        var distinctTypes = message.Metadata.MessageHierarchy.Distinct();
        return string.Join(";", distinctTypes.Select(t => t.AssemblyQualifiedName));
    }

}
#endregion