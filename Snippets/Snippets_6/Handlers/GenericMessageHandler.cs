namespace Snippets6.Handlers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    #region GenericMessageHandler

    public class GenericSynchronousMessageHandler : IHandleMessages<Object>
    {
        static ILog Logger = LogManager.GetLogger(typeof(GenericSynchronousMessageHandler));

        public Task Handle(Object message)
        {
            Logger.Info(string.Format("I just received a message of type {0}.", message.GetType().Name));
            Console.WriteLine("*********************************************************************************");
            SomeLibrary.SomeMethod(message);
            return Task.FromResult(0);
        }
    }
    public class GenericAsynchronousMessageHandler : IHandleMessages<Object>
    {
        static ILog Logger = LogManager.GetLogger(typeof(GenericSynchronousMessageHandler));

        public async Task Handle(Object message)
        {
            Logger.Info(string.Format("I just received a message of type {0}.", message.GetType().Name));
            Console.WriteLine("*********************************************************************************");
            await SomeLibrary.SomeMethodAsync(message);
        }
    }

    #endregion
}


