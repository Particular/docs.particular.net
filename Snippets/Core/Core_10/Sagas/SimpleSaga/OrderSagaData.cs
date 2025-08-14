namespace Core.Sagas.SimpleSaga;

using NServiceBus;

#region simple-saga-data
public class OrderSagaData :
    ContainSagaData
{
    public string OrderId { get; set; }
}
#endregion


public class SagaValidation
{
    void Simple(EndpointConfiguration endpointConfiguration)
    {
        #region disable-shared-state-validation

        endpointConfiguration.Sagas().DisableBestPracticeValidation();

        #endregion
    }
}