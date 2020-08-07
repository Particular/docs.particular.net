namespace Core8.Forwarding
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, IMessageHandlerContext context)
        {
            Task.Run(async () =>
            {
                #region ForwardingMessageFromHandler

                await context.ForwardCurrentMessageTo("destinationQueue@machine")
                    .ConfigureAwait(false);

                #endregion
            });
        }
    }
}

