using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Serialization;
using NServiceBus.Unicast.Messages;

#region serialize-behavior
class SerializeConnector : StageConnector<IOutgoingLogicalMessageContext, IOutgoingPhysicalMessageContext>
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
            IOutgoingPhysicalMessageContext emptyPhysicalMessageContext = this.CreateOutgoingPhysicalMessageContext(new byte[0], context.RoutingStrategies, context);
            await stage(emptyPhysicalMessageContext).ConfigureAwait(false);
            return;
        }

        Type messageType = context.Message.MessageType;
        IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);
        context.Headers[Headers.ContentType] = messageSerializer.ContentType;
        context.Headers[Headers.EnclosedMessageTypes] = SerializeEnclosedMessageTypes(messageType);

        byte[] array = Serialize(messageSerializer, context);
        IOutgoingPhysicalMessageContext physicalMessageContext = this.CreateOutgoingPhysicalMessageContext(array, context.RoutingStrategies, context);
        await stage(physicalMessageContext).ConfigureAwait(false);
    }

    byte[] Serialize(IMessageSerializer messageSerializer, IOutgoingLogicalMessageContext context)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            messageSerializer.Serialize(context.Message.Instance, stream);
            return stream.ToArray();
        }
    }

    string SerializeEnclosedMessageTypes(Type messageType)
    {
        MessageMetadata metadata = messageMetadataRegistry.GetMessageMetadata(messageType);
        IEnumerable<Type> distinctTypes = metadata.MessageHierarchy.Distinct();
        return string.Join(";", distinctTypes.Select(t => t.AssemblyQualifiedName));
    }
}
#endregion