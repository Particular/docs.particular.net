// ReSharper disable RedundantAssignment
namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using NServiceBus.DeliveryConstraints;
    using NServiceBus.Performance.TimeToBeReceived;
    using NServiceBus.Pipeline;

    class TimeToBeReceived
    {
        public TimeToBeReceived()
        {
            IRoutingContext context = null;

            #region SetDeliveryConstraintDiscardIfNotReceivedBefore
            TimeSpan timeToBeReceived = TimeSpan.FromSeconds(25);
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
