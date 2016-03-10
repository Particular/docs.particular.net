namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus;

    public class HandlerOrdering
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6HandlerOrderingWithCode

            endpointConfiguration.ExecuteTheseHandlersFirst(typeof(HandlerB), typeof(HandlerA), typeof(HandlerC));

            #endregion
        }

        public void SpecifyingFirst(EndpointConfiguration endpointConfiguration)
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