#pragma warning disable 618
namespace Core6.Encryption.WireEncryptedProperty
{
    #region MessageWithEncryptedProperty
    using NServiceBus;

    public class MyMessage :
        IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}
