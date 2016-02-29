namespace Snippets6
{
    using NServiceBus;

    public class HandlerOrdering
    {
        public void Simple()
        {
            #region HandlerOrderingWithCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB), typeof(HandlerA), typeof(HandlerC));

            #endregion
        }

        #region HandlerOrderingWithFirst
        public class MySpecifyingFirst : INeedInitialization
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB));
            }
        }
        #endregion

        #region HandlerOrderingWithMultiple
        public class MySpecifyingOrder : INeedInitialization
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB), typeof(HandlerA), typeof(HandlerC));
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