using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region TransportMessageCompressionMutator

public class TransportMessageCompressionMutator :
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages
{
    static ILog log = LogManager.GetLogger("TransportMessageCompressionMutator");

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        log.Info($"transportMessage.Body size before compression: {context.OutgoingBody.Length}");

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
        log.Info($"transportMessage.Body size after compression: {context.OutgoingBody.Length}");
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
            await bigStream.CopyToAsync(bigStreamOut, 81920, context.CancellationToken)
                .ConfigureAwait(false);
            context.Body = bigStreamOut.ToArray();
        }
    }
}

#endregion