namespace Core6.Handlers.DataAccess
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class Simple
    {

        #region BusinessData-Native-NotIdempotent

        public class NonIdempotentHandler :
            IHandleMessages<AddOrderLine>
        {
            IMyOrm orm;

            public NonIdempotentHandler(IMyOrm orm)
            {
                this.orm = orm;
            }

            public Task Handle(AddOrderLine message, IMessageHandlerContext context)
            {
                using (var session = orm.OpenSession())
                {
                    var order = session.Get(message.OrderId);
                    order.AddLine(message.Product, message.Quantity);
                    session.Commit();
                }
                return Task.CompletedTask;
            }
        }

        #endregion

        #region BusinessData-Native-Idempotent

        public class IdempotentHandler :
            IHandleMessages<AddOrderLine>
        {
            IMyOrm orm;

            public IdempotentHandler(IMyOrm orm)
            {
                this.orm = orm;
            }

            public Task Handle(AddOrderLine message, IMessageHandlerContext context)
            {
                using (var session = orm.OpenSession())
                {
                    var order = session.Get(message.OrderId);
                    if (!order.HasLine(message.LineId))
                    {
                        order.AddLine(message.Product, message.Quantity);
                        session.Commit();
                    }
                }
                return Task.CompletedTask;
            }
        }

        #endregion
    }
}