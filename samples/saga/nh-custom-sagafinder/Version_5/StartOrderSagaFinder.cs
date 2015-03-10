using NServiceBus.Persistence.NHibernate;
using NServiceBus.Saga;

namespace Sample
{
	#region CustomSagaFinderNHibernate
	class StartOrderSagaFinder : IFindSagas<OrderSagaData>.Using<StartOrder>
	{
		readonly NHibernateStorageContext storageContext;

		public StartOrderSagaFinder( NHibernateStorageContext storageContext )
		{
			this.storageContext = storageContext;
		}

		public OrderSagaData FindBy( StartOrder message )
		{
			var session = this.storageContext.Session;
			var sagaData = session.QueryOver<OrderSagaData>()
				.Where( d => d.OrderId == message.OrderId )
				.SingleOrDefault();

			//if the instance is null a new saga will be automatically created if
			//the Saga handles the message as IAmStartedByMessages<StartOrder>; otherwise an exception is raised.
			return sagaData;
		}
	}
	#endregion
}
