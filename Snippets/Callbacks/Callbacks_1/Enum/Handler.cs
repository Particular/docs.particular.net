namespace Core6.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region EnumCallbackResponse

    public class Handler : IHandleMessages<Message>
    {
        public async Task Handle(Message message, IMessageHandlerContext context)
        {
            await context.Reply(Status.OK);
        }
    }

    #endregion
}
