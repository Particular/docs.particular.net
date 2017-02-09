namespace Core6.Encryption.EncryptionService
{
    using System;

    #region EncryptionService

    using NServiceBus;
    using NServiceBus.Pipeline;

    public class EncryptionService :
        IEncryptionService
    {
        public EncryptedValue Encrypt(string value, IOutgoingLogicalMessageContext context)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(EncryptedValue encryptedValue, IIncomingLogicalMessageContext context)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
