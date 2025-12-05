namespace Core;

using NServiceBus;

class HandlerOrdering
{
    void Simple(EndpointConfiguration endpointConfiguration)
    {
    }

    void SpecifyingFirst(EndpointConfiguration endpointConfiguration)
    {
        #region HandlerOrderingWithFirst

        endpointConfiguration.AddHandler<HandlerB>();

        #endregion
    }

    void SpecifyingOrder(EndpointConfiguration endpointConfiguration)
    {
        #region HandlerOrderingWithMultiple

        endpointConfiguration.AddHandler<HandlerB>();
        endpointConfiguration.AddHandler<HandlerA>();
        endpointConfiguration.AddHandler<HandlerC>();

        #endregion
    }

    public class HandlerA : IHandleMessages
    {
    }

    public class HandlerB : IHandleMessages
    {
    }

    public class HandlerC : IHandleMessages
    {
    }
}