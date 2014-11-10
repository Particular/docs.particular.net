using System;
using NServiceBus;
using OnlineSales.Contracts.Sales;


namespace OnlineSales.Billing
{
    #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler
    public partial class OrderAcceptedHandler
    {
		
        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Billing received " + message.GetType().Name);

            var submitPayment = new OnlineSales.Internal.Commands.Billing.SubmitPayment();
            Bus.Send(submitPayment);
        }

    }
    #endregion

    public partial class OrderAcceptedHandler
    {
        #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler.handleSubmitPaymentResponse
        partial void HandleImplementation(Internal.Messages.Billing.SubmitPaymentResponse message)
        {
            //Handle the SubmitPaymentResponse
            Console.WriteLine("Billing received " + message.GetType().Name);

            //To access and save to saga data use code like this..
            //Data.AuthCode = message.AuthorizationCode;
        }
        #endregion

        #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler.AllMessagesReceived.signalr
        partial void AllMessagesReceived()
        {
            Console.WriteLine("All messages received. Completing the Saga.");
            MarkAsComplete();
            
            var billingCompleted = new OnlineSales.Contracts.Billing.BillingCompleted();
            Bus.Publish(billingCompleted);
        }
        #endregion
    }
}

namespace OnlineSales.Billing.Example
{
    #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler.before
    public partial class OrderAcceptedHandler
    {

        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Billing received " + message.GetType().Name);
        }

    }
    #endregion

    public partial class OrderAcceptedHandler
    {
        #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler.AllMessagesReceived
        partial void AllMessagesReceived()
        {
            Console.WriteLine("All messages received. Completing the Saga.");
            MarkAsComplete();
        }
        #endregion
    }
    
    public partial class OrderAcceptedHandler
    {
        partial void HandleImplementation(OrderAccepted message);
        partial void AllMessagesReceived();
        void MarkAsComplete()
        {

        }
    }
}

namespace OnlineSales.Billing.Example2
{
    #region ServiceMatrix.OnlinesSales.Billing.OrderAcceptedHandler.full
    public partial class OrderAcceptedHandler
    {
        partial void HandleImplementation(OrderAccepted message)
        {
            // TODO: OrderAcceptedHandler: Add code to handle the OrderAccepted message.
            Console.WriteLine("Billing received {0} for order id {1}", message.GetType().Name, message.OrderId);

            //set the saga order id.  This will be accessable in any future handler.
            Data.OrderId = message.OrderId;

            Console.WriteLine("Submitting Order {0} for payment", Data.OrderId);

            //Pasting the code from the user code pop up here so we send the payment request when the OrderAccepted Event arrives.
            var submitPayment = new OnlineSales.Internal.Commands.Billing.SubmitPayment();
            Bus.Send(submitPayment);
        }

        partial void HandleImplementation(Internal.Messages.Billing.SubmitPaymentResponse message)
        {
            //store the authorization code
            Data.PaymentAuthorizationCode = message.AuthorizationCode;

            System.Console.WriteLine("Payment response received for order {0} with auth code {1}", message.AuthorizationCode, Data.OrderId);
        }

        partial void AllMessagesReceived()
        {
            //Publish the BillingCompleted event.  Assign event values from the saga data values. 
            var billingCompleted = new OnlineSales.Contracts.Billing.BillingCompleted();
            billingCompleted.AuthorizationCode = Data.PaymentAuthorizationCode;
            billingCompleted.OrderId = Data.OrderId;
            Bus.Publish(billingCompleted);

            //Mark this saga as complete and free up the persistence resources.
            System.Console.WriteLine("Marking Saga complete for order {0}", Data.OrderId);
            MarkAsComplete();
        }
    }
    #endregion

    public partial class OrderAcceptedHandler
    {
        IBus Bus;
        public OrderAcceptedHandlerSagaData Data { get; set; }
        partial void HandleImplementation(OrderAccepted message);
        partial void HandleImplementation(Internal.Messages.Billing.SubmitPaymentResponse message);
        partial void AllMessagesReceived();
        void MarkAsComplete()
        {

        }
    }
}