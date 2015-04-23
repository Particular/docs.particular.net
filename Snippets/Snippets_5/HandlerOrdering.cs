using NServiceBus;

public class HandlerOrdering
{
    public void Simple()
    {
        #region HandlerOrderingWithFluent
        
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.LoadMessageHandlers(First<HandlerB>.Then<HandlerA>().AndThen<HandlerC>());

        #endregion
    }

    #region HandlerOrderingWithFirst
    public class MySpecifyingFirst : ISpecifyMessageHandlerOrdering
    {
        public void SpecifyOrder(Order order)
        {
            order.SpecifyFirst<HandlerB>();
        }
    }
    #endregion

    #region HandlerOrderingWithMultiple
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