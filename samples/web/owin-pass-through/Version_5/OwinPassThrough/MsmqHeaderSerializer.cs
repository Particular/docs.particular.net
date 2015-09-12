using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NServiceBus.Transports.Msmq;

#region MsmqHeaderSerializer
static class MsmqHeaderSerializer
{
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
        var serializer = new XmlSerializer(typeof (List<HeaderInfo>));
        using (var stream = new MemoryStream())
        {
            serializer.Serialize(stream, headerInfos);
            return stream.ToArray();
        }
    }
}
#endregion