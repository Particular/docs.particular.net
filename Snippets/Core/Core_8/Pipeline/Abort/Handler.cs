// ReSharper disable UnusedParameter.Local
namespace Core8.Pipeline.Abort
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region AbortHandler

    class Handler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            context.DoNotContinueDispatchingCurrentMessageToHandlers();
            return Task.CompletedTask;
        }
    }

    #endregion
}