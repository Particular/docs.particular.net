namespace Core3.Handlers
{
    using Common;
    using NServiceBus;

    #region CreatingMessageHandler

    public class MyHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            //do something with the message data
            SomeLibrary.SomeMethod(message.Data);
        }
    }

    #endregion

    #region EmptyHandler

    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            // do something in the client process
        }
    }

    #endregion
}


