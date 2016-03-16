namespace Snippets3.Encryption
{
    using NServiceBus;

    public class Usage
    {

        public Usage()
        {
            Configure configure = Configure.With();
            #region EncryptionServiceSimple

            configure.RijndaelEncryptionService();
            #endregion
        }
    }
}
