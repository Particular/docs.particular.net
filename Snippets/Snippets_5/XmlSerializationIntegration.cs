using System.IO;
using System.Xml.Linq;
using NServiceBus;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming

namespace Snippets5
{
    public class XmlSerializationIntegration
    {
        public const string Element =
@"<nutrition>
    <daily-values>
	    <total-fat units='g'>65</total-fat>
	    <saturated-fat units='g'>20</saturated-fat>
	    <cholesterol units='mg'>300</cholesterol>
	    <sodium units='mg'>2400</sodium>
	    <carb units='g'>300</carb>
	    <fiber units='g'>25</fiber>
	    <protein units='g'>50</protein>
    </daily-values>
</nutrition>";

        public const string Document = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                                       + Element;

        public void RawXml()
        {
            var busConfiguration = new BusConfiguration();

            #region ConfigureRawXmlSerialization

            busConfiguration.UseSerialization<XmlSerializer>().DontWrapRawXml();
            // DontWrapSingleMessages() is not required since this has been removed.

            #endregion

            IBus bus = default(IBus);
            #region SendingRawXmlWithXDocument

            bus.SendLocal(new MessageWithXDocument
            {
                nutrition = XDocument.Load(new StringReader(Document)),
            });

            #endregion

            #region SendingRawXmlWithXElement

            bus.SendLocal(new MessageWithXElement
            {
                nutrition = XElement.Load(new StringReader(Element)),
            });

            #endregion
        }

        #region MessageWithXDocument

        public class MessageWithXDocument : IMessage
        {
            public XDocument nutrition { get; set; } // name and casing must match the rootnode
        }

        #endregion

        #region MessageWithXElement

        public class MessageWithXElement : IMessage
        {
            public XElement nutrition { get; set; } // name and casing must match the rootnode
        }

        #endregion
    }
}