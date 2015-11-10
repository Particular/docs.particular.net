namespace Snippets4.Serialization
{
    using NServiceBus;

    public class BinarySerialization
    {
        public void Simple()
        {

            #region BinarySerialization

            Configure.Serialization.Binary();

            #endregion

        }
    }
}