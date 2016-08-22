namespace Core5.Handlers
{
    using NServiceBus;

    #region EmptyHandler

    public class MyMessageHandler :
        IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            // do something in the client process
        }
    }

    #endregion
}


