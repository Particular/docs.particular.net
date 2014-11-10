using System;

#region ServiceMatrix.OnlineSales.Internal.Commands.Billing.SubmitPayment
namespace OnlineSales.Internal.Commands.Billing
{
    public class SubmitPayment
    {
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
#endregion