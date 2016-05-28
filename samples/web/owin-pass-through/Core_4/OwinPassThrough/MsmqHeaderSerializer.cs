using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NServiceBus.Transports.Msmq;

#region MsmqHeaderSerializer
static class MsmqHeaderSerializer
{
    static XmlSerializer serializer = new XmlSerializer(typeof(List<HeaderInfo>));
    public static byte[] CreateHeaders(string messageType)
    {
        var headerInfos = new List<HeaderInfo>
            {
                new HeaderInfo
                {
                    Key = "NServiceBus.EnclosedMessageTypes",
                    Value = messageType
                }
            };
        using (var stream = new MemoryStream())
        {
            serializer.Serialize(stream, headerInfos);
            return stream.ToArray();
        }
    }
}
#endregion