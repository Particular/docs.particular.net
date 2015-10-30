using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

#region deserialize-behavior
class DeserializeBehavior : IBehavior<IncomingContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory logicalMessageFactory;
    static ILog logger = LogManager.GetLogger<DeserializeBehavior>();

    public DeserializeBehavior(SerializationMapper serializationMapper, MessageMetadataRegistry messageMetadataRegistry, LogicalMessageFactory logicalMessageFactory)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
        this.logicalMessageFactory = logicalMessageFactory;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        TransportMessage transportMessage = context.PhysicalMessage;

        if (transportMessage.IsControlMessage())
        {
            logger.Info("Received a control message. Skipping deserialization as control message data is contained in the header.");
            next();
            return;
        }
        try
        {
            context.LogicalMessages = Extract(transportMessage);
        }
        catch (Exception exception)
        {
            throw new MessageDeserializationException(transportMessage.Id, exception);
        }

        next();
    }

    List<LogicalMessage> Extract(TransportMessage physicalMessage)
    {
        if (physicalMessage.Body == null || physicalMessage.Body.Length == 0)
        {
            return new List<LogicalMessage>();
        }

        string typeIdentifier;
        if (!physicalMessage.Headers.TryGetValue(Headers.EnclosedMessageTypes, out typeIdentifier))
        {
            return Deserialize(physicalMessage, new List<MessageMetadata>());
        }
        List<MessageMetadata> messageMetadata = GetMessageMetadata(typeIdentifier)
            .ToList();
        if (messageMetadata.Count == 0 && physicalMessage.MessageIntent != MessageIntentEnum.Publish)
        {
            logger.WarnFormat("Could not determine message type from message header '{0}'. MessageId: {1}", typeIdentifier, physicalMessage.Id);
        }
        return Deserialize(physicalMessage, messageMetadata);
    }

    IEnumerable<MessageMetadata> GetMessageMetadata(string messageTypeIdentifier)
    {
        return messageTypeIdentifier
            .Split(';')
            .Select(type => messageMetadataRegistry.GetMessageMetadata(type))
            .Where(metadata => metadata != null);
    }

    List<LogicalMessage> Deserialize(TransportMessage physicalMessage, List<MessageMetadata> messageMetadata)
    {
        IMessageSerializer messageSerializer = serializationMapper.GetSerializer(physicalMessage.Headers);
        List<Type> messageTypesToDeserialize = messageMetadata.Select(x => x.MessageType).ToList();
        using (MemoryStream stream = new MemoryStream(physicalMessage.Body))
        {
            return messageSerializer.Deserialize(stream, messageTypesToDeserialize)
                .Select(x => logicalMessageFactory.Create(x.GetType(), x, physicalMessage.Headers))
                .ToList();
        }
    }

}
#endregion