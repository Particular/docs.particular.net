using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

namespace Sample
{
	class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
	{
		readonly NHibernateStorageContext storageContext;

		public CompleteOrderSagaFinder( NHibernateStorageContext storageContext )
		{
			this.storageContext = storageContext;
		}

		public OrderSagaData FindBy( CompleteOrder message )
		{
			var session = this.storageContext.Session;
			var sagaData = session.QueryOver<OrderSagaData>()
				.Where( d => d.OrderId == message.OrderId )
				.SingleOrDefault();

			return sagaData;
		}
	}
}
