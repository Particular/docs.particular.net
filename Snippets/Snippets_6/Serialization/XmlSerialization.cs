namespace Snippets5.Serialization
{
    using NServiceBus;

    public class XmlSerialization
    {
        public void Simple()
        {
            #region XmlSerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }
    }
}