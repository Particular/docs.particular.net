namespace Core5.Encryption.EncryptionService
{
    using System;

    #region EncryptionService

    using NServiceBus;
    using NServiceBus.Encryption;

    public class EncryptionService :
        IEncryptionService
    {
        public EncryptedValue Encrypt(string value)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(EncryptedValue encryptedValue)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}