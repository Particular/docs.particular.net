namespace Core6.Encryption.WireEncryptedProperty
{

    #region MessageWithEncryptedProperty
    using NServiceBus;
    using WireEncryptedString = NServiceBus.Encryption.MessageProperty.WireEncryptedString;

    public class MyMessage :
        IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}
