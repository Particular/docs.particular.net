using System;
using NServiceBus;
using OnlineSalesSaga.Internal.Commands.Billing;
using OnlineSalesSaga.Contracts.Sales;
using OnlineSalesSaga.Internal.Messages.Billing;
using NServiceBus.Saga;


namespace OnlineSalesSaga.Billing
{
    public partial class OrderAcceptedHandler
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderAcceptedHandlerSagaData> mapper)
        {
            
            // mapper.ConfigureMapping<SubmitPaymentResponse>(m => /* specify message property m.PropertyThatCorrelatesToSaga */).ToSaga(s => /* specify saga data property s.PropertyThatCorrelatesToMessage */);

            
            // If you add new messages to be handled by your saga, you will need to manually add a call to ConfigureMapping for them.
        }
    }
}
