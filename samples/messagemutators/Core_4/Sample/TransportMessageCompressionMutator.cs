using System.IO;
using System.IO.Compression;
using log4net;
using NServiceBus;
using NServiceBus.MessageMutator;

#region TransportMessageCompressionMutator
public class TransportMessageCompressionMutator : IMutateTransportMessages
{
    static ILog logger = LogManager.GetLogger("TransportMessageCompressionMutator");

    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        logger.Info($"transportMessage.Body size before compression: {transportMessage.Body.Length}");

        var mStream = new MemoryStream(transportMessage.Body);
        var outStream = new MemoryStream();

        using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
        {
            mStream.CopyTo(tinyStream);
        }
        // copy the compressed buffer only after the GZipStream is disposed, 
        // otherwise, not all the compressed message will be copied.
        transportMessage.Body = outStream.ToArray();
        transportMessage.Headers["IWasCompressed"] = "true";
        logger.Info($"transportMessage.Body size after compression: {transportMessage.Body.Length}");
    }

    public void MutateIncoming(TransportMessage transportMessage)
    {
        if (!transportMessage.Headers.ContainsKey("IWasCompressed"))
        {
            return;
        }
        var memoryStream = new MemoryStream(transportMessage.Body);
        using (var bigStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            var bigStreamOut = new MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            transportMessage.Body = bigStreamOut.ToArray();
        }
    }
}
#endregion