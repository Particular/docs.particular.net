using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Unicast.Messages;

#region deserialize-behavior

class DeserializeBehavior :
    IBehavior<IncomingContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory messageFactory;
    static ILog log = LogManager.GetLogger<DeserializeBehavior>();

    public DeserializeBehavior(SerializationMapper serializationMapper, MessageMetadataRegistry messageMetadataRegistry, LogicalMessageFactory messageFactory)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
        this.messageFactory = messageFactory;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        var transportMessage = context.PhysicalMessage;

        if (transportMessage.IsControlMessage())
        {
            log.Info("Received a control message. Skipping deserialization as control message data is contained in the header.");
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
        var messageMetadata = GetMessageMetadata(typeIdentifier)
            .ToList();
        if (messageMetadata.Count != 0 || physicalMessage.MessageIntent == MessageIntentEnum.Publish)
        {
            return Deserialize(physicalMessage, messageMetadata);
        }
        log.Warn($"Could not determine message type from message header '{typeIdentifier}'. MessageId: {physicalMessage.Id}");
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
        var messageSerializer = serializationMapper.GetSerializer(physicalMessage.Headers);
        var typesToDeserialize = messageMetadata.Select(x => x.MessageType)
            .ToList();
        using (var stream = new MemoryStream(physicalMessage.Body))
        {
            return messageSerializer.Deserialize(stream, typesToDeserialize)
                .Select(x => messageFactory.Create(x.GetType(), x, physicalMessage.Headers))
                .ToList();
        }
    }

}

#endregion