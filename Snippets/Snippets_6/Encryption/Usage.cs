namespace Snippets6.Encryption
{
    using NServiceBus;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionServiceSimple

            endpointConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}