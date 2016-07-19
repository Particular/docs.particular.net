namespace Core5.UpgradeGuides._5to6
{
    using Common;
    using NServiceBus;
    using Handlers;

    #region 5to6-messagehandler

    public class UpgradeMessageHandler :
        IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            SomeLibrary.SomeMethod(message.Data);
        }
    }

    #endregion
}


