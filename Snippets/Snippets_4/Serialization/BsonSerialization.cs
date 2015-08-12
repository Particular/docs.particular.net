namespace Snippets4
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