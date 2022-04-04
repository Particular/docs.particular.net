namespace Testing_8.UpgradeGuides._7to8
{
    using System;
    using NServiceBus.DelayedDelivery;
    using NServiceBus.Transport;

    public class TestableBehaviorContext
    {
        class TestableBehaviorContextImp : NServiceBus.Testing.TestableBehaviorContext
        {
        }

        public void UsingDispatchPropertiesInTestableBehaviorContext()
        {
            #region 7to8-adddeliveryconstraint
            var context = new TestableBehaviorContextImp();
            context.Extensions.Set(new DispatchProperties
            {
                DelayDeliveryWith = new DelayDeliveryWith(TimeSpan.FromDays(1)),
            });
            #endregion
        }
    }
}