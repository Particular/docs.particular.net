using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

namespace Sample
{
	class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
	{
		readonly NHibernateStorageContext storageContext;

		public PaymentTransactionCompletedSagaFinder( NHibernateStorageContext storageContext )
		{
			this.storageContext = storageContext;
		}

		public OrderSagaData FindBy( PaymentTransactionCompleted message )
		{
			var session = this.storageContext.Session;
			var sagaData = session.QueryOver<OrderSagaData>()
				.Where( d => d.PaymentTransactionId == message.PaymentTransactionId )
				.SingleOrDefault();

			return sagaData;
		}
	}
}
