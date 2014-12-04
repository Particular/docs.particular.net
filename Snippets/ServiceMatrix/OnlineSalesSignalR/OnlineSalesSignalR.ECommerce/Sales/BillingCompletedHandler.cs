using System;
using NServiceBus;
using OnlineSalesSignalR.Contracts.Billing;


namespace OnlineSalesSignalR.Sales
{
    public partial class BillingCompletedHandler
    {
		
        partial void HandleImplementation(BillingCompleted message)
        {
            // TODO: BillingCompletedHandler: Add code to handle the BillingCompleted message.
            Console.WriteLine("Sales received " + message.GetType().Name);
        }

    }
}
