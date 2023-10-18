namespace Encryption.Conventions
{
    using NServiceBus;

    #region MessageForEncryptionConvention
    public class MyMessage :
        IMessage
    {
        public string MyEncryptedProperty { get; set; }
    }
    #endregion
}
