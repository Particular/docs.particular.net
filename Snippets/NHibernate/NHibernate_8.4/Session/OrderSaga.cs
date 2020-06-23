using System.Threading.Tasks;
using NServiceBus;

namespace NHibernate_8.Session
{
    #region NHibernateAccessingDataViaContextSaga

    public class OrderSaga :
        Saga<OrderSaga.SagaData>,
        IHandleMessages<OrderMessage>
    {
        public Task Handle(OrderMessage message, IMessageHandlerContext context)
        {
            var nhibernateSession = context.SynchronizedStorageSession.Session();
            nhibernateSession.Save(new Order());
            return Task.CompletedTask;
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData :
            ContainSagaData
        {
        }
    }

}