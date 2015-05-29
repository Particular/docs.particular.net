using System;
using System.IO;
using System.Xml.Linq;
using Messages;
using NServiceBus;

class Program
{
    public const string Element =
"<codes>"
+ "  <condition>"
+ "    <code>395</code>"
+ "    <description>Moderate or heavy snow in area with thunder</description>"
+ "    <day_icon>wsymbol_0012_heavy_snow_showers</day_icon>"
+ "    <night_icon>"
+ "      wsymbol_0028_heavy_snow_showers_night</night_icon>"
+ "    </condition>"
+ "</codes>";

    public const string Document = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                                   + Element;

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Default");
        #region ConfigureRawXmlSerialization
        busConfiguration.UseSerialization<XmlSerializer>().DontWrapRawXml().Namespace("http://www.worldweatheronline.com/");
        // DontWrapSingleMessages() is not required since this has been removed.
        #endregion
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            #region SendingRawXmlWithXDocument
            bus.SendLocal(new MessageWithXDocument
            {
                codes = XDocument.Load(new StringReader(Document)),
            });
            #endregion
            #region SendingRawXmlWithXElement
            bus.SendLocal(new MessageWithXElement
            {
                codes = XElement.Load(new StringReader(Element)),
            });
            #endregion

            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
