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
        IncomingMessage incomingMessage = context.Message;

        List<LogicalMessage> messages = ExtractWithExceptionHandling(incomingMessage);

        foreach (LogicalMessage message in messages)
        {
            IIncomingLogicalMessageContext logicalMessageContext = this.CreateIncomingLogicalMessageContext(message, context);
            await stage(logicalMessageContext).ConfigureAwait(false);
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

            if (messageMetadata.Count == 0 && physicalMessage.GetMesssageIntent() != MessageIntentEnum.Publish)
            {
                logger.WarnFormat("Could not determine message type from message header '{0}'. MessageId: {1}", messageTypeIdentifier, physicalMessage.MessageId);
            }
        }

        using (MemoryStream stream = new MemoryStream(physicalMessage.Body))
        {
            IMessageSerializer messageSerializer = serializationMapper.GetSerializer(physicalMessage.Headers);
            List<Type> messageTypes = messageMetadata.Select(metadata => metadata.MessageType).ToList();
            return messageSerializer.Deserialize(stream, messageTypes)
                .Select(x => logicalMessageFactory.Create(x.GetType(), x))
                .ToList();

        }
    }

}

#endregion