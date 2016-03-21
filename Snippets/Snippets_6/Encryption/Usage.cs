namespace Snippets6.Encryption
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionServiceSimple

            endpointConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}