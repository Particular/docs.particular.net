namespace Core4
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