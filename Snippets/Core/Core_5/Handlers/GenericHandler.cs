namespace Core5.Handlers
{
    using NServiceBus;
    using NServiceBus.Logging;

    #region GenericMessageHandler

    public class GenericHandler : IHandleMessages<object>
    {
        static ILog logger = LogManager.GetLogger<GenericHandler>();

        public void Handle(object message)
        {
            logger.InfoFormat("Received a message of type {0}.", message.GetType().Name);
            SomeLibrary.SomeMethod(message);
        }
    }

    #endregion
}