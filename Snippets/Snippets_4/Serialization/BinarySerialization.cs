namespace Snippets4
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