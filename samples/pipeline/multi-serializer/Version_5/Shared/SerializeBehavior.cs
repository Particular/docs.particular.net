using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

class SerializeBehavior : IBehavior<OutgoingContext>
{
    SerializationMapper serializationMapper;

    public SerializeBehavior(SerializationMapper serializationMapper)
    {
        this.serializationMapper = serializationMapper;
    }

    public void Invoke(OutgoingContext context, Action next)
    {
        if (!context.OutgoingMessage.IsControlMessage())
        {
            Serialize(context);

                context.OutgoingMessage.Headers[Headers.EnclosedMessageTypes] = SerializeEnclosedMessageTypes(context.OutgoingLogicalMessage);

            foreach (KeyValuePair<string, string> headerEntry in context.OutgoingLogicalMessage.Headers)
            {
                context.OutgoingMessage.Headers[headerEntry.Key] = headerEntry.Value;
            }
        }

        next();
    }
    #region serialize-behavior
    void Serialize(OutgoingContext context)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            Type messageType = context.OutgoingLogicalMessage.Instance.GetType();
            IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);
            messageSerializer.Serialize(context.OutgoingLogicalMessage.Instance, ms);
            context.OutgoingMessage.Headers[Headers.ContentType] = messageSerializer.ContentType;
            context.OutgoingMessage.Body = ms.ToArray();
        }
    }
    #endregion

    string SerializeEnclosedMessageTypes(LogicalMessage message)
    {
        IEnumerable<Type> distinctTypes = message.Metadata.MessageHierarchy.Distinct();

        return string.Join(";", distinctTypes.Select(t => t.AssemblyQualifiedName));
    }

}