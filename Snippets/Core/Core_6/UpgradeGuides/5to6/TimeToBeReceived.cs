// ReSharper disable RedundantAssignment
namespace Core6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus.DeliveryConstraints;
    using NServiceBus.Performance.TimeToBeReceived;
    using NServiceBus.Pipeline;

    class TimeToBeReceived
    {
        TimeToBeReceived(IRoutingContext context)
        {
            #region SetDeliveryConstraintDiscardIfNotReceivedBefore
            var timeToBeReceived = TimeSpan.FromSeconds(25);
            var deliveryConstraint = new DiscardIfNotReceivedBefore(timeToBeReceived);
            context.Extensions.AddDeliveryConstraint(deliveryConstraint);
            #endregion

            #region ReadDeliveryConstraintDiscardIfNotReceivedBefore
            context.Extensions.TryGetDeliveryConstraint(out var constraint);
            timeToBeReceived = constraint.MaxTime;
            #endregion
        }
    }
}
