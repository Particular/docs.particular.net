namespace Snippets3.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            Configure configure = Configure.With();
            #region JsonSerialization

            configure.JsonSerializer();
        
            #endregion

        }

    }
}