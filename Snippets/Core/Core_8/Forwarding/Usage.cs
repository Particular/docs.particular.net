﻿namespace Core7.Forwarding
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, IMessageHandlerContext context)
        {
            #region ForwardingWithCode
            endpointConfiguration.ForwardReceivedMessagesTo("destinationQueue@machine");
            #endregion

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

