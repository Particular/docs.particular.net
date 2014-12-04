using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;


namespace OnlineSales.Billing
{
    #region ServiceMatrix.OnlineSales.Billing.SubmitPaymentHandler
    public partial class SubmitPaymentHandler
    {
		
        partial void HandleImplementation(SubmitPayment message)
        {
            // TODO: SubmitPaymentHandler: Add code to handle the SubmitPayment message.
            Console.WriteLine("Billing received " + message.GetType().Name);

            var response = new OnlineSales.Internal.Messages.Billing.SubmitPaymentResponse();
            Bus.Reply(response);
        }

    }
    #endregion
}
