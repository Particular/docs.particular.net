namespace Core5.Forwarding
{
    using NServiceBus;

    class UsingIBus
    {
        void Handler(IBus bus)
        {
            #region ForwardingMessageFromHandler

            bus.ForwardCurrentMessageTo("destinationQueue@machine");

            #endregion
        }
    }
}
