namespace Snippets3.Handlers
{
    using System;
    using Common.Logging;
    using NServiceBus;

    #region GenericMessageHandler

    public class GenericMessageHandler : IHandleMessages<Object>
    {
        static ILog Logger = LogManager.GetLogger(typeof(GenericMessageHandler));

        public void Handle(Object message)
        {
            Logger.Info(string.Format("I just received a message of type {0}.", message.GetType().Name));
            Console.WriteLine("*********************************************************************************");
        }
    }

    #endregion
}


