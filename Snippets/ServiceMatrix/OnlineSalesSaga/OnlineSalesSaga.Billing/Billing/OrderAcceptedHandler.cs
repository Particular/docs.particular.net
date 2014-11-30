using System;
using NServiceBus;
using OnlineSalesSaga.Internal.Commands.Billing;
using OnlineSalesSaga.Contracts.Sales;


namespace OnlineSalesSaga.Billing
{
    #region ServiceMatrix.OnlineSalesSaga.Billing.OrderAcceptedHandler
    public partial class OrderAcceptedHandler
    {
		
        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Billing received " + message.GetType().Name);

            var submitPayment = new OnlineSalesSaga.Internal.Commands.Billing.SubmitPayment();
            Bus.Send(submitPayment);
        }

    }
    #endregion

    public partial class OrderAcceptedHandler
    {

        #region ServiceMatrix.OnlineSalesSaga.Billing.OrderAcceptedHandler.handleSubmitPaymentResponse
        partial void HandleImplementation(Internal.Messages.Billing.SubmitPaymentResponse message)
        {
            //Handle the SubmitPaymentResponse
            Console.WriteLine("Billing received " + message.GetType().Name);

            //To access and save to saga data use code like this..
            //Data.PaymentAuthorizationCode = message.AuthorizationCode;
        }
        #endregion

        #region ServiceMatrix.OnlineSalesSaga.Billing.OrderAcceptedHandler.AllMessagesReceived
        partial void AllMessagesReceived()
        {
            Console.WriteLine("All messages received. Completing the Saga.");
            MarkAsComplete();
        }
        #endregion
    }
}
