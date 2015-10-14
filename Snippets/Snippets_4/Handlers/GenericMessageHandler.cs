namespace Snippets4.Handlers
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;

    #region GenericMessageHandler

    public class GenericMessageHandler : IHandleMessages<object>
    {
        static ILog Logger = LogManager.GetLogger(typeof(GenericMessageHandler));

        public void Handle(object message)
        {
            Logger.Info(string.Format("I just received a message of type {0}.", message.GetType().Name));
            Console.WriteLine("*********************************************************************************");
            SomeLibrary.SomeMethod(message);
        }
    }

    #endregion
}


