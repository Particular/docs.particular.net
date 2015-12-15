// ReSharper disable RedundantAssignment
namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;
    using NServiceBus.DeliveryConstraints;
    using NServiceBus.Performance.TimeToBeReceived;

    class TimeToBeReceived
    {
        public TimeToBeReceived()
        {
            RoutingContext context = null;

            #region SetDeliveryConstraintDiscardIfNotReceivedBefore
            var timeToBeReceived = TimeSpan.FromSeconds(25);
            context.AddDeliveryConstraint(new DiscardIfNotReceivedBefore(timeToBeReceived));
            #endregion

            #region ReadDeliveryConstraintDiscardIfNotReceivedBefore
            DiscardIfNotReceivedBefore constraint;
            context.TryGetDeliveryConstraint(out constraint);
            timeToBeReceived = constraint.MaxTime;
            #endregion
        }
    }
}
