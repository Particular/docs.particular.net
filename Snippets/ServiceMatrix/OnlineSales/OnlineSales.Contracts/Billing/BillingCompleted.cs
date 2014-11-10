using System;

namespace OnlineSales.Contracts.Billing
{
    public class BillingCompleted
    {
        public Guid OrderId { get; set; }
        public string AuthorizationCode { get; set; }
    }
}
