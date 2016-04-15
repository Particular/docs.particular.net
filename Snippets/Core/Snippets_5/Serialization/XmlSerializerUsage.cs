namespace Core5.Serialization
{
    using NServiceBus;

    class XmlSerializerUsage
    {
        XmlSerializerUsage(BusConfiguration busConfiguration)
        {
            #region XmlSerialization

            busConfiguration.UseSerialization<XmlSerializer>();

            #endregion
        }

    }
}