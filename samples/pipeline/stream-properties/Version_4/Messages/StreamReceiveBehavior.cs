using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NServiceBus.Gateway.HeaderManagement;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
#pragma warning disable 618

public class StreamReceiveBehavior : IBehavior<ReceiveLogicalMessageContext>
{
    string location;

    public StreamReceiveBehavior(StreamStorageLocation storageLocation)
    {
        location = storageLocation.Location;
    }
    public void Invoke(ReceiveLogicalMessageContext context, Action next)
    {
        object message = context.LogicalMessage.Instance;

        List<FileStream> streamsToCleanUp = new List<FileStream>();
        foreach (PropertyInfo property in StreamDatabusHelpers.GetStreamProperties(message))
        {
            object propertyValue = property.GetValue(message, null);

            string headerKey = StreamDatabusHelpers.GetHeaderKey(message, property);

            string dataBusKey;

            if (!context.LogicalMessage.Headers.TryGetValue(HeaderMapper.DATABUS_PREFIX + headerKey, out dataBusKey))
            {
                continue;
            }

            string filePath = Path.Combine(location, dataBusKey);
            FileStream fileStream = File.OpenRead(filePath);
            property.SetValue(message,fileStream);
            streamsToCleanUp.Add(fileStream);
        }

        next();
        foreach (FileStream fileStream in streamsToCleanUp)
        {
            fileStream.Dispose();
        }
    }

}

public class StreamStorageLocation
{
    public string Location { get; set; }
}