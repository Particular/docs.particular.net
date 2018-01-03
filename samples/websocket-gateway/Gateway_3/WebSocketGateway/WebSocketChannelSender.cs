using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using NServiceBus.Gateway;
using WebSocketSharp;

#region WebScoketGateway-ChannelSender
public class WebSocketChannelSender :
    IChannelSender
{
    public Task Send(string remoteAddress, IDictionary<string, string> headers, Stream data)
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
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, headers);
            data.CopyTo(stream);
            return stream.ToArray();
        }
    }
}
#endregion
