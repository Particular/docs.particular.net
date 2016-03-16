using System.Xml.Linq;
using NServiceBus;

namespace Snippets4
{
    public class XmlSerializationIntegration
    {
        public void RawXml()
        {
            #region ConfigureRawXmlSerialization

            Configure.Serialization.Xml(c => c.DontWrapRawXml()).DontWrapSingleMessages();

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