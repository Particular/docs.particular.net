//------------------------------------------------------------------------------
// This file is copied from the OnlineSales.Billing\Infrastructure\Billing
// It's copied because it's an auto-generated file and the #region's will get
// removed when the file is auto-generated.
//------------------------------------------------------------------------------

using System;
using NServiceBus;
using OnlineSales.Internal.Commands.Billing;
using OnlineSales.Contracts.Sales;
using OnlineSales.Internal.Messages.Billing;
using NServiceBus.Saga;

#region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandler.saga
namespace OnlineSales.Billing
{
    public partial class OrderAcceptedHandler :
        Saga<OrderAcceptedHandlerSagaData>,
        IOrderAcceptedHandler,
        ServiceMatrix.Shared.INServiceBusComponent,
        IAmStartedByMessages<OrderAccepted>,
        IHandleMessages<SubmitPaymentResponse>
    {

        public void Handle(OrderAccepted message)
        {
            // .....
#endregion
            // Store message in Saga Data for later use
            this.Data.OrderAccepted = message;
            // Handle message on partial class
            this.HandleImplementation(message);

            // Check if Saga is Completed 
            CheckIfAllMessagesReceived();
        }

        partial void HandleImplementation(OrderAccepted message);

        public void Send(SubmitPayment message)
        {
            ConfigureSubmitPayment(message);
            Bus.Send(message);
        }

        partial void ConfigureSubmitPayment(SubmitPayment message);

        public void Handle(SubmitPaymentResponse message)
        {
            // Store message in Saga Data for later use
            this.Data.SubmitPaymentResponse = message;

            // Handle message on partial class
            this.HandleImplementation(message);

            // Check if Saga is Completed 
            CheckIfAllMessagesReceived();
        }

        partial void HandleImplementation(SubmitPaymentResponse message);

        public void CheckIfAllMessagesReceived()
        {
            if (this.Data.OrderAccepted != null && this.Data.SubmitPaymentResponse != null)
            {
                AllMessagesReceived();
            }
        }

        partial void AllMessagesReceived();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderAcceptedHandlerSagaData> mapper)
        {

            // mapper.ConfigureMapping<SubmitPaymentResponse>(m => /* specify message property m.PropertyThatCorrelatesToSaga */).ToSaga(s => /* specify saga data property s.PropertyThatCorrelatesToMessage */);


            // If you add new messages to be handled by your saga, you will need to manually add a call to ConfigureMapping for them.
        }
    }

    #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandlerSagaData.auto
    public partial class OrderAcceptedHandlerSagaData : IContainSagaData
    {
        public virtual Guid Id { get; set; }
        public virtual string Originator { get; set; }
        public virtual string OriginalMessageId { get; set; }

        public virtual OrderAccepted OrderAccepted { get; set; }
        public virtual SubmitPaymentResponse SubmitPaymentResponse { get; set; }

    }
    #endregion


    #region ServiceMatrix.OnlineSales.Billing.OrderAcceptedHandlerSagaData.custom
    public partial class OrderAcceptedHandlerSagaData : IContainSagaData
    {
        [Unique]
        public Guid OrderID { get; set; }
        public string PaymentAuthorizationCode { get; set; }
    }
    #endregion

    public partial interface IOrderAcceptedHandler
    {

        void Send(SubmitPayment message);

    }


}

namespace ServiceMatrix.Shared
{
    public interface INServiceBusComponent
    {
    }
}