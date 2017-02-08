namespace Core6.UpgradeGuides.Split.WireEncryptedProperty
{

    #region SplitMessageWithEncryptedProperty

    using NServiceBus;
    using WireEncryptedString = NServiceBus.Encryption.MessageProperty.WireEncryptedString;

    public class MyMessage :
        IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}