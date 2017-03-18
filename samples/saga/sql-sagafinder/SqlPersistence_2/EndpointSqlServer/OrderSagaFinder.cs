using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region sqlServerSagaFinder

class OrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{
    public Task<OrderSagaData> FindBy(PaymentTransactionCompleted message, SynchronizedStorageSession session, ReadOnlyContextBag context)
    {
        return session.GetSagaData<OrderSagaData>(
            context: context,
            whereClause: "JSON_VALUE(Data,'$.PaymentTransactionId') = @propertyValue",
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