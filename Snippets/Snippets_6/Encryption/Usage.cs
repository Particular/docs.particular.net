namespace Snippets6.Encryption
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EncryptionServiceSimple

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.RijndaelEncryptionService();

            #endregion
        }


    }
}