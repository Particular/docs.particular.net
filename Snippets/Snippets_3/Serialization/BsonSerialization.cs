namespace Snippets3.Serialization
{
    using NServiceBus;

    public class BsonSerialization
    {
        public void Simple()
        {

            #region BsonSerialization

            Configure configure = Configure.With();
            configure.BsonSerializer();

            #endregion
        }

    }
}