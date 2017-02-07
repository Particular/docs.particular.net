namespace Core5.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region EncryptionFromIEncryptionService

            busConfiguration.RegisterEncryptionService(
                func: builder =>
                {
                    return new EncryptionService();
                });

            #endregion
        }

    }
}