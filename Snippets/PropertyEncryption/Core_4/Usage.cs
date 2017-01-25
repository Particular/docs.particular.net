namespace Core4.Encryption
{
    using NServiceBus;

    class Usage
    {

        Usage(Configure configure)
        {
            #region EncryptionServiceSimple

            configure.RijndaelEncryptionService();

            #endregion
        }
    }
}
