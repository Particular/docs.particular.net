namespace Snippets5.UpgradeGuides._5to6
{
    using NServiceBus;

    public class HandlerOrdering
    {
        public void Simple(BusConfiguration busConfiguration)
        {
            #region 5to6HandlerOrderingWithCode

            busConfiguration.LoadMessageHandlers(First<HandlerB>.Then<HandlerA>().AndThen<HandlerC>());

            #endregion
        }

        #region 5to6HandlerOrderingWithFirst
        public class MySpecifyingFirst : ISpecifyMessageHandlerOrdering
        {
            public void SpecifyOrder(Order order)
            {
                order.SpecifyFirst<HandlerB>();
            }
        }
        #endregion

        public class HandlerA
        {
        
        }
        public class HandlerB
        {
        
        }
        public class HandlerC
        {
        
        }
    }
}