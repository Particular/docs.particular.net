using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region MySqlFinder
class OrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<CompletePaymentTransaction>
{
    public Task<OrderSagaData> FindBy(CompletePaymentTransaction message, SynchronizedStorageSession session, ReadOnlyContextBag context)
    {
        return session.GetSagaData<OrderSagaData>(
            context: context,
            whereClause: "JSON_EXTRACT(Data,'$.PaymentTransactionId') = @propertyValue",
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