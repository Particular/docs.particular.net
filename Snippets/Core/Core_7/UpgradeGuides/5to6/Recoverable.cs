﻿namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.DeliveryConstraints;
    using NServiceBus.Pipeline;

    class Recoverable
    {
        Recoverable(IRoutingContext context)
        {
            #region SetDeliveryConstraintNonDurable

            context.Extensions.AddDeliveryConstraint(new NonDurableDelivery());

            #endregion

            #region ReadDeliveryConstraintNonDurable

            NonDurableDelivery constraint;
            context.Extensions.TryGetDeliveryConstraint(out constraint);

            #endregion
        }
    }
}