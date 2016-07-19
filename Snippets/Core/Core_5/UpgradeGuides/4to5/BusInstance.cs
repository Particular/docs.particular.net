namespace Core5.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;
    #region 4to5businstance
    public class BusInstance :
        IWantToRunWhenBusStartsAndStops
    {
        public BusInstance(IBus bus, IBuilder builder)
        {
            Bus = bus;
            Builder = builder;
        }

        public static IBuilder Builder;
        public static IBus Bus;
        #endregion
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}