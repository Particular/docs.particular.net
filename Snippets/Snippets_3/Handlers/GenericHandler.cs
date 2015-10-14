namespace Snippets3.Handlers
{
    using Common.Logging;
    using NServiceBus;

    #region GenericMessageHandler
    public class GenericHandler : IHandleMessages<object>
    {
        static ILog Logger = LogManager.GetLogger(typeof(GenericHandler));

        public void Handle(object message)
        {
            Logger.Info(string.Format("Received a message of type {0}.", message.GetType().Name));
            SomeLibrary.SomeMethod(message);
        }
    }

    #endregion
}