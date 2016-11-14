namespace Core4.Serialization
{
    using NServiceBus;

    class XmlSerializerUsage
    {
        XmlSerializerUsage()
        {
            #region XmlSerialization

            Configure.Serialization.Xml();

            #endregion
        }

    }
}