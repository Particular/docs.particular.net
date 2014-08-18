using NServiceBus;

public class HandlerOrdering
{
    public void Simple()
    {
        #region HandlerOrderingWithFluentV5

        Configure.With(b => 
            b.LoadMessageHandlers(First<HandlerB>.Then<HandlerA>().AndThen<HandlerC>()));

        #endregion
    }

    #region HandlerOrderingWithFirstV5
    public class MySpecifyingFirst : ISpecifyMessageHandlerOrdering
    {
        public void SpecifyOrder(Order order)
        {
            order.SpecifyFirst<HandlerB>();
        }
    }
    #endregion

    #region HandlerOrderingWithMultipleV5
    public class MySpecifyingOrder : ISpecifyMessageHandlerOrdering
    {
        public void SpecifyOrder(Order order)
        {
            order.Specify(typeof(HandlerB),typeof(HandlerA),typeof(HandlerC));
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