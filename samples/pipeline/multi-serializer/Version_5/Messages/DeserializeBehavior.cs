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

class DeserializeBehavior : IBehavior<IncomingContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory logicalMessageFactory;

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

        string messageTypeIdentifier;
        List<MessageMetadata> messageMetadata = new List<MessageMetadata>();

        if (physicalMessage.Headers.TryGetValue(Headers.EnclosedMessageTypes, out messageTypeIdentifier))
        {
            foreach (string messageTypeString in messageTypeIdentifier.Split(';'))
            {
                string typeString = messageTypeString;
                MessageMetadata metadata = messageMetadataRegistry.GetMessageMetadata(typeString);
                if (metadata == null)
                {
                    continue;
                }
                messageMetadata.Add(metadata);
            }

            if (messageMetadata.Count == 0 && physicalMessage.MessageIntent != MessageIntentEnum.Publish)
            {
                log.WarnFormat("Could not determine message type from message header '{0}'. MessageId: {1}", messageTypeIdentifier, physicalMessage.Id);
            }
        }

        return Deserialize(physicalMessage, messageMetadata);
    }

    #region deserialize-behavor
    List<LogicalMessage> Deserialize(TransportMessage physicalMessage, List<MessageMetadata> messageMetadata)
    {
        using (MemoryStream stream = new MemoryStream(physicalMessage.Body))
        {
            IMessageSerializer messageSerializer = serializationMapper.GetSerializer(physicalMessage.Headers);
            List<Type> messageTypesToDeserialize = messageMetadata.Select(metadata => metadata.MessageType).ToList();
            return messageSerializer.Deserialize(stream, messageTypesToDeserialize)
                .Select(x => logicalMessageFactory.Create(x.GetType(), x, physicalMessage.Headers))
                .ToList();
        }
    }
    #endregion


    static ILog log = LogManager.GetLogger(typeof(DeserializeBehavior));
}