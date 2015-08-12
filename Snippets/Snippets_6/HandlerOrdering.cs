namespace Snippets6
{
    using NServiceBus;

    public class HandlerOrdering
    {
        public void Simple()
        {
            #region HandlerOrderingWithCode

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB), typeof(HandlerA), typeof(HandlerC));

            #endregion
        }

        #region HandlerOrderingWithFirst
        public class MySpecifyingFirst : INeedInitialization
        {
            public void Customize(BusConfiguration configuration)
            {
                configuration.ExecuteTheseHandlersFirst(typeof(HandlerB));
            }
        }
        #endregion

        #region HandlerOrderingWithMultiple
        public class MySpecifyingOrder : INeedInitialization
        {
            public void Customize(BusConfiguration configuration)
            {
                configuration.ExecuteTheseHandlersFirst(typeof(HandlerB), typeof(HandlerA), typeof(HandlerC));
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