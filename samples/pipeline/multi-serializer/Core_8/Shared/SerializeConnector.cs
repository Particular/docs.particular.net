using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

#region serialize-behavior

class SerializeConnector :
    StageConnector<IOutgoingLogicalMessageContext, IOutgoingPhysicalMessageContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;

    public SerializeConnector(
        SerializationMapper serializationMapper,
        MessageMetadataRegistry messageMetadataRegistry)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
    }

    public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> stage)
    {
        if (context.ShouldSkipSerialization())
        {
            var emptyMessageContext = this.CreateOutgoingPhysicalMessageContext(
                messageBody: new byte[0],
                routingStrategies: context.RoutingStrategies,
                sourceContext: context);
            await stage(emptyMessageContext)
                .ConfigureAwait(false);
            return;
        }

        var messageType = context.Message.MessageType;
        var messageSerializer = serializationMapper.GetSerializer(messageType);
        var headers = context.Headers;
        headers[Headers.ContentType] = messageSerializer.ContentType;
        headers[Headers.EnclosedMessageTypes] = SerializeEnclosedMessageTypes(messageType);

        var array = Serialize(messageSerializer, context);
        var physicalMessageContext = this.CreateOutgoingPhysicalMessageContext(
            messageBody: array,
            routingStrategies: context.RoutingStrategies,
            sourceContext: context);
        await stage(physicalMessageContext)
            .ConfigureAwait(false);
    }

    byte[] Serialize(IMessageSerializer messageSerializer, IOutgoingLogicalMessageContext context)
    {
        using (var stream = new MemoryStream())
        {
            messageSerializer.Serialize(context.Message.Instance, stream);
            return stream.ToArray();
        }
    }

    string SerializeEnclosedMessageTypes(Type messageType)
    {
        var metadata = messageMetadataRegistry.GetMessageMetadata(messageType);
        var distinctTypes = metadata.MessageHierarchy.Distinct();
        return string.Join(";", distinctTypes.Select(t => t.AssemblyQualifiedName));
    }
}

#endregion