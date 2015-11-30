using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

#region serialize-behavior
using ToContext = NServiceBus.OutgoingPipeline.OutgoingPhysicalMessageContext;
using FromContext = NServiceBus.Pipeline.OutgoingPipeline.OutgoingLogicalMessageContext;
class SerializeConnector : StageConnector<FromContext, ToContext>
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

    public override async Task Invoke(FromContext context, Func<ToContext, Task> next)
    {
        if (context.ShouldSkipSerialization())
        {
            await next(new ToContext(new byte[0], context.RoutingStrategies, context)).ConfigureAwait(false);
            return;
        }

        Type messageType = context.Message.MessageType;
        IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);
        context.Headers[Headers.ContentType] = messageSerializer.ContentType;
        context.Headers[Headers.EnclosedMessageTypes] = SerializeEnclosedMessageTypes(messageType);

        var array = Serialize(messageSerializer, context);
        await next(new ToContext(array, context.RoutingStrategies, context)).ConfigureAwait(false);
    }

    byte[] Serialize(IMessageSerializer messageSerializer, FromContext context)
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