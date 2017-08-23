using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using NServiceBus.Gateway;
using WebSocketSharp;

namespace WebSocketGateway
{
    #region WebScoketGateway-ChannelSender
    public class WebSocketChannelSender : IChannelSender
    {
        public Task Send(string remoteAddress, IDictionary<string, string> headers, Stream data)
        {
            using (var ws = new WebSocket(remoteAddress))
            {
                ws.Connect();
                var bytes = GetBytes(headers, data);
                ws.Send(bytes);
                ws.Close();
            }
            return Task.CompletedTask;
        }

        private static byte[] GetBytes(IDictionary<string, string> headers, Stream data)
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
}
