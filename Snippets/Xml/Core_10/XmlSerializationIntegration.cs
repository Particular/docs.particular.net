using System.Xml.Linq;
using NServiceBus;

class XmlSerializationIntegration
{
    public void RawXml(EndpointConfiguration endpointConfiguration)
    {
        #region ConfigureRawXmlSerialization

        var serialization = endpointConfiguration.UseSerialization<XmlSerializer>();
        serialization.DontWrapRawXml();
        #endregion
    }

    #region MessageWithXDocument

    public class MessageWithXDocument : IMessage
    {
        // name and casing must match the root node
        public XDocument nutrition { get; set; }
    }
    #endregion

    #region MessageWithXElement

    public class MessageWithXElement : IMessage
    {
        // name and casing must match the root node
        public XElement nutrition { get; set; }
    }
    #endregion
}