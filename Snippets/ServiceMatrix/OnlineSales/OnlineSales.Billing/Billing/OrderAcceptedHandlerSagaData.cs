using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;
using OnlineSales.Contracts.Sales;
using OnlineSales.Internal.Messages.Billing;
using NServiceBus.Saga;


#region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandlerSagaData
namespace OnlineSales.Billing
{
    public partial class OrderAcceptedHandlerSagaData
    {
        //Put your own custom properties here.  
        public Guid OrderId { get; set; }
        public string PaymentAuthorizationCode { get; set; }
    }
}
#endregion