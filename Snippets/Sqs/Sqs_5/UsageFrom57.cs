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


class UsageFrom57
{
    void DoNotWrapOutgoingMessages(EndpointConfiguration endpointConfiguration)
    {
        #region DoNotWrapOutgoingMessages [5.7,6.0)
        var transport = endpointConfiguration.UseTransport<SqsTransport>();
        transport.DoNotWrapOutgoingMessages();
        #endregion
    }
}