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
using ToContext = NServiceBus.Pipeline.Contexts.LogicalMessagesProcessingStageBehavior.Context;
using FromContext = NServiceBus.PhysicalMessageProcessingStageBehavior.Context;

class DeserializeBehavior : StageConnector<FromContext, ToContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory logicalMessageFactory;

    public DeserializeBehavior(
        SerializationMapper serializationMapper,
        MessageMetadataRegistry messageMetadataRegistry,
        LogicalMessageFactory logicalMessageFactory)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
        this.logicalMessageFactory = logicalMessageFactory;
    }


    public override void Invoke(FromContext context, Action<ToContext> next)
    {
        var transportMessage = context.GetPhysicalMessage();

        if (transportMessage.IsControlMessage())
        {
            log.Info("Received a control message. Skipping deserialization as control message data is contained in the header.");

            next(new ToContext(Enumerable.Empty<LogicalMessage>(), context));
            return;
        }
        var messages = ExtractWithExceptionHandling(transportMessage);
        next(new ToContext(messages, context));
    }

    List<LogicalMessage> ExtractWithExceptionHandling(TransportMessage transportMessage)
    {
        try
        {
            return Extract(transportMessage);
        }
        catch (Exception exception)
        {
            throw new MessageDeserializationException(transportMessage.Id, exception);
        }
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
            log.WarnFormat("Could not determine message type from message header '{0}'. MessageId: {1}", typeIdentifier, physicalMessage.Id);
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
        List<Type> messageTypes = messageMetadata.Select(x => x.MessageType).ToList();
        using (MemoryStream stream = new MemoryStream(physicalMessage.Body))
        {
            return messageSerializer.Deserialize(stream, messageTypes)
                .Select(x => logicalMessageFactory.Create(x.GetType(), x))
                .ToList();
        }
    }


    static ILog log = LogManager.GetLogger(typeof(DeserializeBehavior));
}

#endregion