using System.IO.Compression;
using Microsoft.Extensions.Logging;
using NServiceBus.MessageMutator;

#region TransportMessageCompressionMutator

public class TransportMessageCompressionMutator(ILogger<TransportMessageCompressionMutator> logger) :
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages
{

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        logger.LogInformation("transportMessage.Body size before compression: {Length}", context.OutgoingBody.Length);

        var mStream = new MemoryStream(context.OutgoingBody.ToArray(), false);
        var outStream = new MemoryStream();

        using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
        {
            mStream.CopyTo(tinyStream);
        }
        // copy the compressed buffer only after the GZipStream is disposed,
        // otherwise, not all the compressed message will be copied.
        context.OutgoingBody = outStream.ToArray();
        context.OutgoingHeaders["IWasCompressed"] = "true";
        logger.LogInformation("transportMessage.Body size after compression: {Length}", context.OutgoingBody.Length);
        return Task.CompletedTask;
    }

    public async Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        if (!context.Headers.ContainsKey("IWasCompressed"))
        {
            return;
        }
        var memoryStream = new MemoryStream(context.Body.ToArray());
        using (var bigStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            var bigStreamOut = new MemoryStream();
            await bigStream.CopyToAsync(bigStreamOut, 81920, context.CancellationToken);
            context.Body = bigStreamOut.ToArray();
        }
    }
}

#endregion