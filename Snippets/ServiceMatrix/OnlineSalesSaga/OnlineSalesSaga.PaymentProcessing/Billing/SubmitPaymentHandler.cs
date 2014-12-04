using System;
using NServiceBus;
using OnlineSalesSaga.Internal.Commands.Billing;


namespace OnlineSalesSaga.Billing
{
    #region ServiceMatrix.OnlineSalesSaga.Billing.SubmitPaymentHandler
    public partial class SubmitPaymentHandler
    {
		
        partial void HandleImplementation(SubmitPayment message)
        {
            // TODO: SubmitPaymentHandler: Add code to handle the SubmitPayment message.
            Console.WriteLine("Billing received " + message.GetType().Name);

            var response = new OnlineSalesSaga.Internal.Messages.Billing.SubmitPaymentResponse();
            Bus.Reply(response);
        }

    }
    #endregion
}
