namespace Core5
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