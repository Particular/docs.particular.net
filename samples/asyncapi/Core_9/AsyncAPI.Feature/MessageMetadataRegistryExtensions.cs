using System.Reflection;
using NServiceBus.Unicast.Messages;

namespace AsyncAPI.Feature;

static class MessageMetadataRegistryExtensions
{
//TODO can this be improved?
    public static IEnumerable<MessageMetadata> GetAllMessages(this MessageMetadataRegistry registry)
    {
        var methodInfo =
            typeof(MessageMetadataRegistry).GetMethod("GetAllMessages", BindingFlags.Instance | BindingFlags.NonPublic);
        return (IEnumerable<MessageMetadata>) methodInfo.Invoke(registry, Array.Empty<object>());
    }
}