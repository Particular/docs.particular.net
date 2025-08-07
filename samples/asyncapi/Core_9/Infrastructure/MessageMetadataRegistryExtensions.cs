using System.Reflection;
using NServiceBus.Unicast.Messages;

namespace Infrastructure;

static class MessageMetadataRegistryExtensions
{
    // This is still a bit ugly
    public static IEnumerable<MessageMetadata> GetAllMessages(this MessageMetadataRegistry registry)
    {
        var methodInfo =
            typeof(MessageMetadataRegistry).GetMethod("GetAllMessages", BindingFlags.Instance | BindingFlags.NonPublic);
        return (IEnumerable<MessageMetadata>) methodInfo.Invoke(registry, Array.Empty<object>());
    }
}