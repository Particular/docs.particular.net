namespace Snippets4.Serialization
{
    using NServiceBus;

    public class JsonSerialization
    {
        public void Simple()
        {

            #region JsonSerialization

            Configure.Serialization.Json();
        
            #endregion

        }

    }
}