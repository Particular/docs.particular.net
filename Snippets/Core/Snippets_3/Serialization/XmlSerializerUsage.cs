namespace Snippets3.Serialization
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