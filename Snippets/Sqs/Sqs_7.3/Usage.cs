using System;
using NServiceBus;

public class Usage
{
    void DisablePayloadSigning(EndpointConfiguration endpointConfiguration, string bucketName, string keyPrefix)
    {
        #region DisablePayloadSigning
        var transport = new SqsTransport
        {
            S3 = new S3Settings(bucketName, keyPrefix)
            {
                DisablePayloadSigning = true
            }
        };

        endpointConfiguration.UseTransport(transport);
        #endregion
    }

    void MessageVisibilityTimeout(EndpointConfiguration endpointConfiguration)
    {
        #region MessageVisibilityTimeout [7.3,)
        var transport = new SqsTransport
        {
            MessageVisibilityTimeout = TimeSpan.FromMinutes(5)
        };

        endpointConfiguration.UseTransport(transport);
        #endregion
    }

    void ReserveBytesInMessageSizeCalculation(EndpointConfiguration endpointConfiguration)
    {
        #region ReserveBytesInMessageSizeCalculation
        var transport = new SqsTransport
        {
            ReserveBytesInMessageSizeCalculation = 5*1024 // 5KB for additional metadata
        };

        endpointConfiguration.UseTransport(transport);
        #endregion
    }

    void MaxAutoMessageVisibilityRenewalDuration(EndpointConfiguration endpointConfiguration)
    {
        #region MaxAutoMessageVisibilityRenewalDuration
        var transport = new SqsTransport
        {
            MaxAutoMessageVisibilityRenewalDuration = TimeSpan.FromMinutes(15)
        };

        endpointConfiguration.UseTransport(transport);
        #endregion
    }
}