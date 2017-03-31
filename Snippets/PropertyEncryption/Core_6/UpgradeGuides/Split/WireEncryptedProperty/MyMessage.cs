#pragma warning disable 618
namespace Core6.UpgradeGuides.Split.WireEncryptedProperty
{
    #region SplitMessageWithEncryptedProperty
    using NServiceBus;

    public class MyMessage :
        IMessage
    {
        public WireEncryptedString MyEncryptedProperty { get; set; }
    }
    #endregion
}
