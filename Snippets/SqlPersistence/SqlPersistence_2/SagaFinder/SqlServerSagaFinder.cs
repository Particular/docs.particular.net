namespace SagaFinder
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Sagas;

    #region SagaFinder-sqlServer

    class SqlServerSagaFinder :
        IFindSagas<MySagaData>.Using<MyMessage>
    {
        public Task<MySagaData> FindBy(MyMessage message, SynchronizedStorageSession session, ReadOnlyContextBag context)
        {
            return session.GetSagaData<MySagaData>(
                context: context,
                whereClause: "JSON_VALUE(Data,'$.PropertyPathInJson') = @propertyValue",
                appendParameters: (builder, append) =>
                {
                    var parameter = builder();
                    parameter.ParameterName = "propertyValue";
                    parameter.Value = message.PropertyValue;
                    append(parameter);
                });
        }
    }

    #endregion
}