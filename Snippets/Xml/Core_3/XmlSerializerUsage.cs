namespace Core3
{
    using NServiceBus;

    class XmlSerializerUsage
    {
        XmlSerializerUsage(Configure configure)
        {
            #region XmlSerialization

            configure.XmlSerializer();

            #endregion
        }

    }
}