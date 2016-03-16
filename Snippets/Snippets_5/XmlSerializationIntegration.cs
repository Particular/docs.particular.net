using System.Xml.Linq;
using NServiceBus;

namespace Snippets5
{
    public class XmlSerializationIntegration
    {

        public void RawXml()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ConfigureRawXmlSerialization

            busConfiguration.UseSerialization<XmlSerializer>()
                .DontWrapRawXml();
            #endregion
        }


        #region MessageWithXDocument

        public class MessageWithXDocument : IMessage
        {
            // name and casing must match the rootnode
            public XDocument nutrition { get; set; } 
        }
        #endregion

        #region MessageWithXElement

        public class MessageWithXElement : IMessage
        {
            // name and casing must match the rootnode
            public XElement nutrition { get; set; } 
        }
        #endregion
    }
}