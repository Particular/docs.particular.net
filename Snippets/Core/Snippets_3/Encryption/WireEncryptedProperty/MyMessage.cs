namespace Core3.Encryption.WireEncryptedProperty
{
    using NServiceBus;

    #region MessageWithEncryptedProperty
    public class MyMessage : IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}
