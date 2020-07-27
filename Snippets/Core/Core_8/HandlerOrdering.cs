namespace Core8
{
    using NServiceBus;

    class HandlerOrdering
    {
        void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region HandlerOrderingWithCode

            endpointConfiguration.ExecuteTheseHandlersFirst(
                typeof(HandlerB),
                typeof(HandlerA),
                typeof(HandlerC));

            #endregion
        }

        void SpecifyingFirst(EndpointConfiguration endpointConfiguration)
        {
            #region HandlerOrderingWithFirst

            endpointConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB));

            #endregion
        }

        void SpecifyingOrder(EndpointConfiguration endpointConfiguration)
        {
            #region HandlerOrderingWithMultiple

            endpointConfiguration.ExecuteTheseHandlersFirst(
                typeof(HandlerB),
                typeof(HandlerA),
                typeof(HandlerC));

            #endregion
        }

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