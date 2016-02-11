namespace Snippets4.Serialization
{
    using NServiceBus;

    public class BsonSerializerUsage
    {
        public void Simple()
        {

            #region BsonSerialization

            Configure.Serialization.Bson();

            #endregion
        }

    }
}