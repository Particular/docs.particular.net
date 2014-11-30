using System;
using NServiceBus;
using OnlineSalesSignalR.Contracts.Sales;


namespace OnlineSalesSignalR.Billing
{
    public partial class OrderAcceptedHandler
    {
		
        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Billing received " + message.GetType().Name);

            var billingCompleted = new OnlineSalesSignalR.Contracts.Billing.BillingCompleted();
            Bus.Publish(billingCompleted);
        }

    }
}
