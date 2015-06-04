namespace Snippets5.Encryption.Conventions
{
    using NServiceBus;

    #region MessageForEncryptionConvention
    public class Message : IMessage
    {
        public string MyEncryptedProperty { get; set; }
    }
    #endregion
}
