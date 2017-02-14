using NServiceBus;
using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

namespace NHibernate_6.Session
{

    #region NHibernateAccessingDataViaContextSaga

    public class OrderSaga :
        Saga<OrderSaga.SagaData>,
        IHandleMessages<OrderMessage>
    {
        NHibernateStorageContext dataContext;

        public OrderSaga(NHibernateStorageContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Handle(OrderMessage message)
        {
            var nhibernateSession = dataContext.Session;
            nhibernateSession.Save(new Order());
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