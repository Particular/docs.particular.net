namespace Snippets3.Handlers
{
    using NServiceBus;

    #region CreatingMessageHandler

    public class MyHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            //do something relevant with the message
            SomeLibrary.SomeMethod(message);
        }
    }

    #endregion
}


