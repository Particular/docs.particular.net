namespace Snippets3.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            #region JsonSerialization

            Configure configure = Configure.With();
            configure.JsonSerializer();
        
            #endregion

        }

    }
}