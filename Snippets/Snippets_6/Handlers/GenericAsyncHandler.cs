namespace Snippets6.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    #region GenericMessageHandler
    
    public class GenericAsyncHandler : IHandleMessages<object>
    {
        static ILog Logger = LogManager.GetLogger(typeof(GenericAsyncHandler));

        public async Task Handle(object message)
        {
            Logger.Info(string.Format("Received a message of type {0}.", message.GetType().Name));
            await SomeLibrary.SomeMethodAsync(message);
        }
    }

    #endregion
}