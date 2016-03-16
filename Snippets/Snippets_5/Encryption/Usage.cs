namespace Snippets5.Encryption
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region EncryptionServiceSimple

            busConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}