namespace Core6.Handlers
{
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.Logging;

    #region GenericMessageHandler

    public class GenericAsyncHandler : IHandleMessages<object>
    {
        static ILog logger = LogManager.GetLogger<GenericAsyncHandler>();

        public Task Handle(object message, IMessageHandlerContext context)
        {
            logger.InfoFormat($"Received a message of type {message.GetType().Name}.");
            return SomeLibrary.SomeAsyncMethod(message);
        }
    }

    #endregion
}