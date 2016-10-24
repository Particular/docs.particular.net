namespace Core5.Handlers
{
    using NServiceBus;

    public class Simple
    {

        #region BusinessData-Native-NotIdempotent

        public class NonIdempotentHandler : IHandleMessages<AddOrderLine>
        {
            IMyOrm orm;

            public NonIdempotentHandler(IMyOrm orm)
            {
                this.orm = orm;
            }

            public void Handle(AddOrderLine message)
            {
                using (var session = orm.OpenSession())
                {
                    var order = session.Get(message.OrderId);
                    order.AddLine(message.Product, message.Quantity);
                    session.Commit();
                }
            }
        }

        #endregion

        #region BusinessData-Native-Idempotent

        public class IdempotentHandler : IHandleMessages<AddOrderLine>
        {
            IMyOrm orm;

            public IdempotentHandler(IMyOrm orm)
            {
                this.orm = orm;
            }

            public void Handle(AddOrderLine message)
            {
                using (var session = orm.OpenSession())
                {
                    var order = session.Get(message.OrderId);
                    if (order.HasLine(message.LineId))
                    {
                        return;
                    }

                    order.AddLine(message.Product, message.Quantity);
                    session.Commit();
                }
            }
        }

        #endregion
    }
}