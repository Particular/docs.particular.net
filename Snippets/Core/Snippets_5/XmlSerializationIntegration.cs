using System.Xml.Linq;
using NServiceBus;

namespace Core5
{
    class XmlSerializationIntegration
    {

        void RawXml(BusConfiguration busConfiguration)
        {
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