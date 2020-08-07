namespace Core7.Forwarding
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, IMessageHandlerContext context)
        {
#pragma warning disable 618
            #region ForwardingWithCode
            endpointConfiguration.ForwardReceivedMessagesTo("destinationQueue@machine");
            #endregion
#pragma warning restore 618

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

