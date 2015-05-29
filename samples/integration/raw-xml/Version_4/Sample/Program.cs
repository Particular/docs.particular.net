using System;
using System.IO;
using System.Xml.Linq;
using Messages;
using NServiceBus;
using NServiceBus.Installation.Environments;

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
        #region ConfigureRawXmlSerialization
        Configure.Serialization.Xml(c => c.DontWrapRawXml().Namespace("http://www.worldweatheronline.com/")).DontWrapSingleMessages();
        #endregion
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Integration.RawXml");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

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