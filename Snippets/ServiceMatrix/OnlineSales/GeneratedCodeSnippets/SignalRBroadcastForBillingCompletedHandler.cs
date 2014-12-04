//------------------------------------------------------------------------------
// This file is copied from the OnlineSales.ECommerce\Infrastructure\Sales
// It's copied because it's an auto-generated file and the #region's will get
// removed when the file is auto-generated.
//------------------------------------------------------------------------------

using NServiceBus;
using Microsoft.AspNet.SignalR;
using OnlineSales.Contracts.Billing;

namespace OnlineSales.Sales
{
    #region ServiceMatrix.OnlineSales.Sales.BroadcastBillingCompleted.auto
    public class BroadcastBillingCompleted : IHandleMessages<BillingCompleted>
    {
        public void Handle(BillingCompleted message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<BillingCompletedHub>();
            context.Clients.All.billingCompleted(message);
        }
    }
    #endregion

    #region ServiceMatrix.OnlineSales.Sales.BillingCompletedHub.auto
    public class BillingCompletedHub : Hub
    {
    }
    #endregion
}