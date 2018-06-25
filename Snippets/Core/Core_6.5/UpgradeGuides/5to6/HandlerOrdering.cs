namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;

    class HandlerOrdering
    {
        HandlerOrdering(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6HandlerOrderingWithCode

            endpointConfiguration.ExecuteTheseHandlersFirst(
                typeof(HandlerB),
                typeof(HandlerA),
                typeof(HandlerC));

            #endregion
        }

        void SpecifyingFirst(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6HandlerOrderingWithFirst

            endpointConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB));

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