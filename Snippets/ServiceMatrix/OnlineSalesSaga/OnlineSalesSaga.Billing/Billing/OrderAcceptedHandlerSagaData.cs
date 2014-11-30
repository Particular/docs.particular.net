using System;
using NServiceBus;
using OnlineSalesSaga.Internal.Commands.Billing;
using OnlineSalesSaga.Contracts.Sales;
using OnlineSalesSaga.Internal.Messages.Billing;
using NServiceBus.Saga;


#region ServiceMatrix.OnlineSalesSaga.Billing.OrderAcceptedHandlerSagaData
namespace OnlineSalesSaga.Billing
{
    public partial class OrderAcceptedHandlerSagaData
    {
        //Put your own custom properties here.  
        public Guid OrderId { get; set; }
        public string PaymentAuthorizationCode { get; set; }
    }
}
#endregion