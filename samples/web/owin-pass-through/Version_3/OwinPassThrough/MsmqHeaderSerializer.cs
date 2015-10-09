using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NServiceBus.Unicast.Queuing.Msmq;

#region MsmqHeaderSerializer
static class MsmqHeaderSerializer
{
    static XmlSerializer serializer = new XmlSerializer(typeof(List<HeaderInfo>));
    public static byte[] CreateHeaders(string messageType)
    {
        List<HeaderInfo> headerInfos = new List<HeaderInfo>
            {
                new HeaderInfo
                {
                    Key = "NServiceBus.EnclosedMessageTypes",
                    Value = messageType
                }
            };
        using (MemoryStream stream = new MemoryStream())
        {
            serializer.Serialize(stream, headerInfos);
            return stream.ToArray();
        }
    }
}
#endregion