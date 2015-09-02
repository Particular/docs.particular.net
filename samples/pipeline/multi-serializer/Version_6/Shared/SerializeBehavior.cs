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
using ToContext = NServiceBus.OutgoingPipeline.PhysicalOutgoingContextStageBehavior.Context;
class SerializeBehavior : StageConnector<OutgoingContext, ToContext>
{
    SerializationMapper serializationMapper;
    MessageMetadataRegistry messageMetadataRegistry;

    public SerializeBehavior(
        SerializationMapper serializationMapper, 
        MessageMetadataRegistry messageMetadataRegistry)
    {
        this.serializationMapper = serializationMapper;
        this.messageMetadataRegistry = messageMetadataRegistry;
    }

    public override void Invoke(OutgoingContext context, Action<ToContext> next)
    {
        object messageInstance = context.GetMessageInstance();
        Type messageType = messageInstance.GetType();
        IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);

        context.SetHeader(Headers.ContentType, messageSerializer.ContentType);
        context.SetHeader(Headers.EnclosedMessageTypes, SerializeMessageTypes(messageType));

        byte[] array = Serialize(messageSerializer, messageInstance);
        next(new ToContext(array, context));
    }

    static byte[] Serialize(IMessageSerializer messageSerializer, object messageInstance)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            messageSerializer.Serialize(messageInstance, ms);
            return ms.ToArray();
        }
    }

    string SerializeMessageTypes(Type messageType)
    {
        var metadata = messageMetadataRegistry.GetMessageMetadata(messageType);
        var distinctTypes = metadata.MessageHierarchy.Distinct();

        return string.Join(";", distinctTypes.Select(t => t.AssemblyQualifiedName));
    }

}
#endregion