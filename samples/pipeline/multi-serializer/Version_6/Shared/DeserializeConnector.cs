using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

#region deserialize-behavior
using ToContext = NServiceBus.Pipeline.Contexts.LogicalMessageProcessingContext;
using FromContext = NServiceBus.PhysicalMessageProcessingContext;

class DeserializeConnector : StageConnector<FromContext, ToContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory logicalMessageFactory;

    public DeserializeConnector(
        SerializationMapper serializationMapper,
        MessageMetadataRegistry messageMetadataRegistry,
        LogicalMessageFactory logicalMessageFactory)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
        this.logicalMessageFactory = logicalMessageFactory;
    }


    public override async Task Invoke(FromContext context, Func<LogicalMessageProcessingContext, Task> next)
    {
        var transportMessage = context.Message;
        var messages = ExtractWithExceptionHandling(transportMessage);
        foreach (var message in messages)
        {
            await next(new ToContext(message, context.Message.Headers, context)).ConfigureAwait(false);
        }
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


    static ILog log = LogManager.GetLogger(typeof(DeserializeConnector));
}

#endregion