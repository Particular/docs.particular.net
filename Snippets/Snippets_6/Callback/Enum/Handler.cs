namespace Snippets5.Callback.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region EnumCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        IBus bus;

        public Handler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(Message message)
        {
            await bus.ReplyAsync(Status.OK);
        }
    }

    #endregion
}
