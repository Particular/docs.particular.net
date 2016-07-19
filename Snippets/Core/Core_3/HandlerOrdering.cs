namespace Core3
{
    using NServiceBus;

    class HandlerOrdering
    {
        void Simple(Configure configure)
        {
            #region HandlerOrderingWithCode

            var configUnicastBus = configure.UnicastBus();
            configUnicastBus.LoadMessageHandlers(First<HandlerB>.Then<HandlerA>().AndThen<HandlerC>());

            #endregion
        }

        #region HandlerOrderingWithFirst
        public class MySpecifyingFirst :
            ISpecifyMessageHandlerOrdering
        {
            public void SpecifyOrder(Order order)
            {
                order.SpecifyFirst<HandlerB>();
            }
        }
        #endregion

        #region HandlerOrderingWithMultiple
        public class MySpecifyingOrder :
            ISpecifyMessageHandlerOrdering
        {
            public void SpecifyOrder(Order order)
            {
                order.Specify(First<HandlerB>.Then<HandlerA>().AndThen<HandlerC>());
            }
        }
        #endregion

        class HandlerA
        {
        }
        class HandlerB
        {
        }
        class HandlerC
        {
        }
    }
}