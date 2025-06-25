namespace Core9.Sagas.SimpleSaga
{
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
            #region disable-shared-state-validation [9.0,)

            endpointConfiguration.Sagas().DisableBestPracticeValidation();

            #endregion
        }
    }
}
