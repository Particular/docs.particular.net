namespace Core8.Handlers.DataAccess
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    public class ManualIdempotence : IHandleMessages<MyMessage>
    {
        #region BusinessData-ManualIdempotence

        public async Task Handle(MyMessage message,
            IMessageHandlerContext context)
        {
            if (IsDuplicate(message))
            {
                return;
            }
            await context.Send(new MyOutgoingMessage());
            await ModifyState();
        }

        #endregion

        Task ModifyState()
        {
            throw new NotImplementedException();
        }

        bool IsDuplicate(MyMessage message)
        {
            throw new NotImplementedException();
        }

        public class MyOutgoingMessage : IMessage
        {
        }
    }
}