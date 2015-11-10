namespace Snippets4.Serialization
{
    using NServiceBus;

    public class BsonSerialization
    {
        public void Simple()
        {

            #region BsonSerialization

            Configure.Serialization.Bson();

            #endregion
        }

    }
}