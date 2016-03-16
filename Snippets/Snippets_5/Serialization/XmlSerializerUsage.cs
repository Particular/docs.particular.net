namespace Snippets5.Serialization
{
    using NServiceBus;

    public class XmlSerializerUsage
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region XmlSerialization

            busConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }

    }
}