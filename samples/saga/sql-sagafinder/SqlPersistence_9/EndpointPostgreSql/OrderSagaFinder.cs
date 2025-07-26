using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region PostgreSqlFinder
class OrderSagaFinder :
    ISagaFinder<OrderSagaData, CompletePaymentTransaction>
{
    public Task<OrderSagaData> FindBy(CompletePaymentTransaction message, ISynchronizedStorageSession storageSession, IReadOnlyContextBag context, CancellationToken cancellationToken = default)
    {
        return storageSession.GetSagaData<OrderSagaData>(
            context: context,
            whereClause: @"""Data""->>'PaymentTransactionId' = @propertyValue",
            appendParameters: (builder, append) =>
            {
                var parameter = builder();
                parameter.ParameterName = "propertyValue";
                parameter.Value = message.PaymentTransactionId;
                append(parameter);
            });
    }
}
#endregion