using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;
using Raven.Client.UniqueConstraints;

namespace Sample
{
	class CompleteOrderSagaFinder : IFindSagas<OrderSagaData>.Using<CompleteOrder>
	{
		readonly ISessionProvider sessionProvider;

		public CompleteOrderSagaFinder( ISessionProvider sessionProvider )
		{
			this.sessionProvider = sessionProvider;
		}

		public OrderSagaData FindBy( CompleteOrder message )
		{
			var session = this.sessionProvider.Session;
			var sagaData = session.LoadByUniqueConstraint<OrderSagaData>( d => d.OrderId, message.OrderId );

			return sagaData;
		}
	}
}
