using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Amazon.SQS.Model;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport.SQS;


class UsageFrom61
{
    void DoNotWrapOutgoingMessages(EndpointConfiguration endpointConfiguration)
    {
        #region DoNotWrapOutgoingMessages [6.1,)
        var transport = new SqsTransport
        {
            DoNotWrapOutgoingMessages = true
        };

        endpointConfiguration.UseTransport(transport);
        #endregion
    }
}