using NServiceBus.RavenDB.Persistence;
using NServiceBus.Saga;
using Raven.Client.UniqueConstraints;

namespace Sample
{
	#region PaymentTransactionCompletedSagaFinderRavenDB
	class PaymentTransactionCompletedSagaFinder : IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
	{
		readonly ISessionProvider sessionProvider;

		public PaymentTransactionCompletedSagaFinder( ISessionProvider sessionProvider )
		{
			this.sessionProvider = sessionProvider;
		}

		public OrderSagaData FindBy( PaymentTransactionCompleted message )
		{
			var session = this.sessionProvider.Session;
			var sagaData = session.LoadByUniqueConstraint<OrderSagaData>( d => d.PaymentTransactionId, message.PaymentTransactionId );

			return sagaData;
		}
	}
	#endregion
}
