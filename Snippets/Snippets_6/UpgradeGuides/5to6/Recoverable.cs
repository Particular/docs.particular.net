namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.DeliveryConstraints;

    class Recoverable
    {
        public Recoverable()
        {
            RoutingContext context = null;

            #region SetDeliveryConstraintNonDurable
            context.AddDeliveryConstraint(new NonDurableDelivery());
            #endregion

            #region ReadDeliveryConstraintNonDurable
            NonDurableDelivery constraint;
            context.TryGetDeliveryConstraint(out constraint);
            #endregion
        }
    }
}