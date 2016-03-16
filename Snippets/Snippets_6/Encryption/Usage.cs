namespace Snippets6.Encryption
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region EncryptionServiceSimple

            endpointConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}