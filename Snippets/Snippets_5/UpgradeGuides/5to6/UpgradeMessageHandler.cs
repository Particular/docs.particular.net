namespace Snippets5.Handlers
{
    using NServiceBus;

    #region 5to6-messagehandler

    public class UpgradeMessageHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            SomeLibrary.SomeMethod(message.Data);
        }
    }

    #endregion
}


