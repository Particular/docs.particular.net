using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;
using NServiceBus.Sagas;

#region sqlServerSagaFinder
class OrderSagaFinder :
    IFindSagas<OrderSagaData>.Using<PaymentTransactionCompleted>
{
    public Task<OrderSagaData> FindBy(PaymentTransactionCompleted message, SynchronizedStorageSession session, ReadOnlyContextBag context)
    {
        return session.FindBy<OrderSagaData>(
            context: context,
            addParameters: command =>
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = "propertyValue";
                parameter.Value = message.PaymentTransactionId;
                command.Parameters.Add(parameter);
            },
            endpointName: "Samples.SqlSagaFinder.SqlServer",
            whereClause: "JSON_VALUE(Data,'$.PaymentTransactionId') = @propertyValue");
    }
}
#endregion