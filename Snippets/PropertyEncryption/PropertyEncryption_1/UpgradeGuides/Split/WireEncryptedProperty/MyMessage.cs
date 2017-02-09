namespace Core6.UpgradeGuides.Split.WireEncryptedProperty
{

    #region SplitMessageWithEncryptedProperty

    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    public class MyMessage :
        IMessage
    {
        public EncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}