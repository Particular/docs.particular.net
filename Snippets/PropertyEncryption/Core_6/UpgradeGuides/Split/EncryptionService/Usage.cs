namespace Core6.UpgradeGuides.Split.EncryptionService
{
    using Core6.Encryption.EncryptionService;
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region SplitEncryptionFromIEncryptionService

            endpointConfiguration.RegisterEncryptionService(
                func: () =>
                {
                    return new EncryptionService();
                });

            #endregion
        }

    }
}