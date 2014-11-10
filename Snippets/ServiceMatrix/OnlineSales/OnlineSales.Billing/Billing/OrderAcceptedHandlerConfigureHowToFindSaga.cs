using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;
using OnlineSales.Contracts.Sales;
using OnlineSales.Contracts.Billing;
using OnlineSales.Internal.Messages.Billing;
using NServiceBus.Saga;


namespace OnlineSales.Billing
{
    #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler.findsaga
    public partial class OrderAcceptedHandler
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderAcceptedHandlerSagaData> mapper)
        {
            //m represents the message and s represents the saga data in this mapping method. 

            mapper.ConfigureMapping<OrderAccepted>(m => m.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<BillingCompleted>(m => m.OrderId).ToSaga(s => s.OrderId);

            // If you add new messages to be handled by your saga, you will need to manually add a call to ConfigureMapping for them.
        }
    }
    #endregion
}
