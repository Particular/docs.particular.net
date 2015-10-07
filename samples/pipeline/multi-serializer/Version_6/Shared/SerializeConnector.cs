using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.OutgoingPipeline;
using NServiceBus.Pipeline;
using NServiceBus.Serialization;
using NServiceBus.TransportDispatch;
using NServiceBus.Unicast.Messages;

#region serialize-behavior
using ToContext = NServiceBus.OutgoingPipeline.OutgoingPhysicalMessageContext;
using FromContext = NServiceBus.Pipeline.Contexts.OutgoingLogicalMessageContext;
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

    public override async Task Invoke(FromContext context, Func<OutgoingPhysicalMessageContext, Task> next)
    {

        object messageInstance = context.Message.Instance;
        Type messageType = messageInstance.GetType();
        IMessageSerializer messageSerializer = serializationMapper.GetSerializer(messageType);

        context.SetHeader(Headers.ContentType, messageSerializer.ContentType);
        context.SetHeader(Headers.EnclosedMessageTypes, SerializeMessageTypes(messageType));

        byte[] array = Serialize(messageSerializer, messageInstance);
        await next(new ToContext(array, context));
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