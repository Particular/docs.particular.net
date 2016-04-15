namespace Snippets4.Serialization
{
    using NServiceBus;

    class BsonSerializerUsage
    {
        BsonSerializerUsage()
        {
            #region BsonSerialization

            Configure.Serialization.Bson();

            #endregion
        }

    }
}