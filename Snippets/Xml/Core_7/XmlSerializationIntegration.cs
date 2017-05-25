namespace Core7
{
    using System.Xml.Linq;
    using NServiceBus;

    class XmlSerializationIntegration
    {

        void RawXml(EndpointConfiguration endpointConfiguration)
        {
            #region ConfigureRawXmlSerialization

            var serialization = endpointConfiguration.UseSerialization<XmlSerializer>();
            serialization.DontWrapRawXml();
            #endregion
        }


        #region MessageWithXDocument

        public class MessageWithXDocument :
            IMessage
        {
            // name and casing must match the rootnode
            public XDocument nutrition { get; set; }
        }
        #endregion

        #region MessageWithXElement

        public class MessageWithXElement :
            IMessage
        {
            // name and casing must match the rootnode
            public XElement nutrition { get; set; }
        }
        #endregion
    }
}