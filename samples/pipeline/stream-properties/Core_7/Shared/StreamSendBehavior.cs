using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus.DeliveryConstraints;
using NServiceBus.Performance.TimeToBeReceived;
using NServiceBus.Pipeline;
#region SendBehaviorDefinition
class StreamSendBehavior :
    Behavior<IOutgoingLogicalMessageContext>
{
    TimeSpan maxMessageTimeToLive = TimeSpan.FromDays(14);
    string location;

    public StreamSendBehavior(StreamStorageSettings storageSettings)
    {
        location = Path.GetFullPath(storageSettings.Location);
    }
    public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
#endregion
        #region copy-stream-properties-to-disk
        var timeToBeReceived = TimeSpan.MaxValue;
        DiscardIfNotReceivedBefore constraint;

        if (context.Extensions.TryGetDeliveryConstraint(out constraint))
        {
            timeToBeReceived = constraint.MaxTime;
        }

        var message = context.Message.Instance;

        foreach (var property in StreamStorageHelper.GetStreamProperties(message))
        {
            var sourceStream = (Stream)property.GetValue(message, null);

            // Ignore null stream properties
            if (sourceStream == null)
            {
                continue;
            }
            var fileKey = GenerateKey(timeToBeReceived);

            var filePath = Path.Combine(location, fileKey);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var target = File.OpenWrite(filePath))
            {
                await sourceStream.CopyToAsync(target)
                    .ConfigureAwait(false);
            }

            // Reset the property to null so no other serializer attempts to use the property
            property.SetValue(message, null);

            // Dispose of the stream
            sourceStream.Dispose();

            // Store the header so on the receiving endpoint the file name is known
            var headerKey = StreamStorageHelper.GetHeaderKey(message, property);
            context.Headers[$"NServiceBus.PropertyStream.{headerKey}"] = fileKey;
        }

        await next()
            .ConfigureAwait(false);
        #endregion
    }

    #region generate-key-for-stream
    string GenerateKey(TimeSpan timeToBeReceived)
    {
        if (timeToBeReceived > maxMessageTimeToLive)
        {
            timeToBeReceived = maxMessageTimeToLive;
        }

        var keepMessageUntil = DateTime.MaxValue;

        if (timeToBeReceived < TimeSpan.MaxValue)
        {
            keepMessageUntil = DateTime.Now + timeToBeReceived;
        }

        return Path.Combine(keepMessageUntil.ToString("yyyy-MM-dd_HH"), Guid.NewGuid().ToString());
    }
    #endregion

}