using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Transport;
using NServiceBus.Unicast.Messages;

#region deserialize-behavior

class DeserializeConnector :
    StageConnector<IIncomingPhysicalMessageContext, IIncomingLogicalMessageContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory logicalMessageFactory;
    static ILog log = LogManager.GetLogger<DeserializeConnector>();

    public DeserializeConnector(
        SerializationMapper serializationMapper,
        MessageMetadataRegistry messageMetadataRegistry,
        LogicalMessageFactory logicalMessageFactory)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
        this.logicalMessageFactory = logicalMessageFactory;
    }

    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<IIncomingLogicalMessageContext, Task> stage)
    {
        var incomingMessage = context.Message;

        var messages = ExtractWithExceptionHandling(incomingMessage);

        foreach (var message in messages)
        {
            var logicalMessageContext = this.CreateIncomingLogicalMessageContext(message, context);
            await stage(logicalMessageContext)
                .ConfigureAwait(false);
        }
    }

    List<LogicalMessage> ExtractWithExceptionHandling(IncomingMessage message)
    {
        try
        {
            return Extract(message);
        }
        catch (Exception exception)
        {
            throw new MessageDeserializationException(message.MessageId, exception);
        }
    }

    List<LogicalMessage> Extract(IncomingMessage physicalMessage)
    {
        if (physicalMessage.Body == null || physicalMessage.Body.Length == 0)
        {
            return new List<LogicalMessage>();
        }

        var messageMetadata = new List<MessageMetadata>();

        var headers = physicalMessage.Headers;
        if (headers.TryGetValue(Headers.EnclosedMessageTypes, out var messageTypeIdentifier))
        {
            foreach (var messageTypeString in messageTypeIdentifier.Split(';'))
            {
                var typeString = messageTypeString;
                var metadata = messageMetadataRegistry.GetMessageMetadata(typeString);
                if (metadata == null)
                {
                    continue;
                }

                messageMetadata.Add(metadata);
            }

            if (
                messageMetadata.Count == 0 &&
                physicalMessage.GetMessageIntent() != MessageIntentEnum.Publish)
            {
                log.Warn($"Could not determine message type from message header '{messageTypeIdentifier}'. MessageId: {physicalMessage.MessageId}");
            }
        }

        using (var stream = new MemoryStream(physicalMessage.Body))
        {
            var messageSerializer = serializationMapper.GetSerializer(headers);
            var typesToDeserialize = messageMetadata
                .Select(metadata => metadata.MessageType)
                .ToList();
            return messageSerializer.Deserialize(stream, typesToDeserialize)
                .Select(x => logicalMessageFactory.Create(x.GetType(), x))
                .ToList();
        }
    }
}

#endregion
