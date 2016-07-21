using System.Xml.Linq;
using NServiceBus;

namespace Core4
{
    class XmlSerializationIntegration
    {
        void RawXml()
        {
            #region ConfigureRawXmlSerialization

            var serialization = Configure.Serialization.Xml(
                customSettings: settings =>
                {
                    settings.DontWrapRawXml();
                });
            serialization.DontWrapSingleMessages();

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