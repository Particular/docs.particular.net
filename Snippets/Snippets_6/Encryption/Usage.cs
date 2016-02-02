namespace Snippets5.Encryption
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EncryptionServiceSimple

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}