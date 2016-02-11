namespace Snippets4.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            #region JsonSerialization

            Configure.Serialization.Json();
        
            #endregion

        }

    }
}