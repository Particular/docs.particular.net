namespace Core5.Encryption
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region EncryptionServiceSimple

            busConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}