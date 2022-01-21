namespace Shared
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.Toolkit.HighPerformance;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Routing;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class StoreAuditBodyInAzureBlobStorageBehavior : Behavior<IDispatchContext>
    {
        public StoreAuditBodyInAzureBlobStorageBehavior(BlobContainerClient blobContainerClient)
        {
            this.blobContainerClient = blobContainerClient;
        }

        public override async Task Invoke(IDispatchContext context, Func<Task> next)
        {
            foreach (var operation in context.Operations)
            {
                var unicastAddress = operation.AddressTag as UnicastAddressTag;

                if (unicastAddress?.Destination != "audit")
                {
                    continue;
                }

                var message = operation.Message;

                var blob = blobContainerClient.GetBlobClient(message.MessageId);
                var options = new BlobUploadOptions
                {
                    Metadata = new Dictionary<string, string>()
                        {
                        { "ContentType", message.Headers[Headers.ContentType] },
                        { "BodySize", message.Body.Length.ToString()}
                        }
                };

                await blob.UploadAsync(
                    BinaryData.FromStream(message.Body.AsStream()),
                    options,
                    context.CancellationToken
                    ).ConfigureAwait(false);

                operation.Message.UpdateBody(null); //TODO: Would ReadOnlyMemory<byte>.Empty be better?
            }

            await next();
        }

        BlobContainerClient blobContainerClient;
    }
}