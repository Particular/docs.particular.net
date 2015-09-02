using System;
using System.IO;
using System.Linq;
using NServiceBus;
using NServiceBus.OutgoingPipeline;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Serialization;
using NServiceBus.TransportDispatch;
using NServiceBus.Unicast.Messages;

#region serialize-behavior
class SerializeBehavior : StageConnector<OutgoingContext, PhysicalOutgoingContextStageBehavior.Context>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;

    public SerializeBehavior(SerializationMapper serializationMapper, MessageMetadataRegistry messageMetadataRegistry)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
    }

    public override void Invoke(OutgoingContext context, Action<PhysicalOutgoingContextStageBehavior.Context> next)
    {
        object messageInstance = context.GetMessageInstance();
        Type messageType = messageInstance.GetType();
        IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);

        context.SetHeader(Headers.ContentType, messageSerializer.ContentType);
        context.SetHeader(Headers.EnclosedMessageTypes, SerializeEnclosedMessageTypes(messageType));

        byte[] array = Serialize(messageSerializer, messageInstance);
        next(new PhysicalOutgoingContextStageBehavior.Context(array, context));
    }

    static byte[] Serialize(IMessageSerializer messageSerializer, object messageInstance)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            messageSerializer.Serialize(messageInstance, ms);
            return ms.ToArray();
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