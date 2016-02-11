namespace Snippets6.Encryption
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EncryptionServiceSimple

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.RijndaelEncryptionService();

            #endregion
        }


    }
}