namespace Snippets5.Callback.Object
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region ObjectCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(Message message)
        {
            await bus.ReplyAsync(new ResponseMessage
            {
                Property = "PropertyValue"
            });
        }
    }

    #endregion
}

