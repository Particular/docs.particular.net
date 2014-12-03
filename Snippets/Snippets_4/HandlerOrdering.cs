using NServiceBus;

public class HandlerOrdering
{
    public void Simple()
    {
        #region HandlerOrderingWithFluent 4

        Configure.With()
            .UnicastBus()
            .LoadMessageHandlers(First<HandlerB>.Then<HandlerA>().AndThen<HandlerC>());

        #endregion
    }

    #region HandlerOrderingWithFirst 4
    public class MySpecifyingFirst : ISpecifyMessageHandlerOrdering
    {
        public void SpecifyOrder(Order order)
        {
            order.SpecifyFirst<HandlerB>();
        }
    }
    #endregion

    #region HandlerOrderingWithMultiple 4
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