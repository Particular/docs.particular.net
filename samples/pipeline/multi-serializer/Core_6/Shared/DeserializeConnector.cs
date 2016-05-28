using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Transports;
using NServiceBus.Unicast.Messages;

#region deserialize-behavior
class DeserializeConnector : StageConnector<IIncomingPhysicalMessageContext, IIncomingLogicalMessageContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;
    LogicalMessageFactory logicalMessageFactory;
    static ILog logger = LogManager.GetLogger<DeserializeConnector>();

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

        List<LogicalMessage> messages = ExtractWithExceptionHandling(incomingMessage);

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

        string messageTypeIdentifier;
        List<MessageMetadata> messageMetadata = new List<MessageMetadata>();

        if (physicalMessage.Headers.TryGetValue(Headers.EnclosedMessageTypes, out messageTypeIdentifier))
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

            if (messageMetadata.Count == 0 && physicalMessage.GetMesssageIntent() != MessageIntentEnum.Publish)
            {
                logger.Warn($"Could not determine message type from message header '{messageTypeIdentifier}'. MessageId: {physicalMessage.MessageId}");
            }
        }

        using (var stream = new MemoryStream(physicalMessage.Body))
        {
            var messageSerializer = serializationMapper.GetSerializer(physicalMessage.Headers);
            List<Type> messageTypes = messageMetadata.Select(metadata => metadata.MessageType).ToList();
            return messageSerializer.Deserialize(stream, messageTypes)
                .Select(x => logicalMessageFactory.Create(x.GetType(), x))
                .ToList();

        }
    }

}

#endregion