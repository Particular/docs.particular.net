namespace Core6.Encryption.WireEncryptedProperty
{

    #region MessageWithEncryptedProperty
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    public class MyMessage :
        IMessage
    {
        public EncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}