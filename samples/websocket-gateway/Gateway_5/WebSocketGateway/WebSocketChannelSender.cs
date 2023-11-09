using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Gateway;
using WebSocketSharp;

#region WebSocketGateway-ChannelSender
public class WebSocketChannelSender :
    IChannelSender
{
    public Task Send(string remoteAddress, IDictionary<string, string> headers, Stream data, CancellationToken cancellationToken = new CancellationToken())
    {
        using (var webSocket = new WebSocket(remoteAddress))
        {
            webSocket.Connect();
            var bytes = GetBytes(headers, data);
            webSocket.Send(bytes);
            webSocket.Close();
        }
        return Task.CompletedTask;
    }

    static byte[] GetBytes(IDictionary<string, string> headers, Stream data)
    {
        using (var stream = new MemoryStream())
        {
            using (var br = new BinaryReader(data))
            {
                var envelope = new ChannelEnvelope
                {
                    Headers = headers,
                    //Data = br.ReadBytes((int)data.Length)
                };

                JsonSerializer.Serialize(stream, headers);
            }

            return stream.ToArray();
        }
    }
}
#endregion
