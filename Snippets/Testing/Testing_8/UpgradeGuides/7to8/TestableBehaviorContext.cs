namespace Testing_8.UpgradeGuides._7to8
{
    using System;
    using NServiceBus.DelayedDelivery;
    using NServiceBus.Transport;

    public class TestableBehaviorContext
    {
        #region 7to8-adddeliveryconstraint
        public void UsingDispatchPropertiesInTestableBehaviorContext(NServiceBus.Testing.TestableBehaviorContext context)
        {
            context.Extensions.Set(new DispatchProperties
            {
                DelayDeliveryWith = new DelayDeliveryWith(TimeSpan.FromDays(1))
            });
        }
        #endregion
    }
}