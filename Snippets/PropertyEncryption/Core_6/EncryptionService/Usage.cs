#pragma warning disable 618
namespace Core6.Encryption.EncryptionService
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromIEncryptionService

            endpointConfiguration.RegisterEncryptionService(
                func: () =>
                {
                    return new EncryptionService();
                });

            #endregion
        }

    }
}