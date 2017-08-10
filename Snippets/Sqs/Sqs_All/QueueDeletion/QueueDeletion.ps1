﻿#Requires -Modules AWSPowerShell

Set-StrictMode -Version 3.0

Function Usage
{
# startcode sqs-delete-queues-endpoint-usage-powershell
# For NServiceBus 6 Endpoints
DeleteQueuesForEndpoint -EndpointName "myendpoint" -QueueNamePrefix "PROD"

# For NServiceBus 5 and below Endpoints
DeleteQueuesForEndpoint -EndpointName "myendpoint" -QueueNamePrefix "PROD" -IncludeRetries
# endcode

# startcode sqs-delete-queues-shared-usage-powershell
DeleteQueue -QueueName "error" -QueueNamePrefix "PROD"
DeleteQueue -QueueName "audit" -QueueNamePrefix "PROD"
# endcode
}

# startcode sqs-delete-queues-for-endpoint-powershell
Function DeleteQueuesForEndpoint
{
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $EndpointName,

[Parameter(Mandatory=$false)]
[string] $QueueNamePrefix,

[Parameter(HelpMessage="Only required for NSB Versions 5 and below")]
[Switch] $IncludeRetries
)

# main queue
DeleteQueue -QueueName $EndpointName -QueueNamePrefix $QueueNamePrefix

# timeout queue
DeleteQueue -QueueName "$EndpointName.Timeouts" -QueueNamePrefix $QueueNamePrefix

# timeout dispatcher queue
DeleteQueue -QueueName "$EndpointName.TimeoutsDispatcher" -QueueNamePrefix $QueueNamePrefix

# retries queue
if ($IncludeRetries) {
DeleteQueue -QueueName "$EndpointName.Retries" -QueueNamePrefix $QueueNamePrefix
}
}
# endcode

# startcode sqs-delete-queues-powershell
Function DeleteQueue
{
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $QueueName,

[Parameter(Mandatory=$false)]
[string] $QueueNamePrefix
)
[string]$name = [QueueNameHelper]::GetSqsQueueName($QueueName, $QueueNamePrefix)
$queueUrl = Get-SQSQueueUrl $name
Remove-SQSQueue -QueueUrl $queueUrl -Force
}

# endcode

# startcode sqs-delete-all-queues-powershell
Function DeleteAllQueues
{
param(

[Parameter(Mandatory=$false)]
[string] $QueueNamePrefix

)
Get-SQSQueue $QueueNamePrefix | Remove-SQSQueue -Force
}
# endcode

Add-Type @'
    using System;

    public static class QueueNameHelper
    {
        public static string GetSqsQueueName(string destination, string queueNamePrefix = null)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException("destination");
            }

            var s = queueNamePrefix + destination;

            // SQS queue names can only have alphanumeric characters, hyphens and underscores.
            // Any other characters will be replaced with a hyphen.
            for (var i = 0; i < s.Length; ++i)
            {
                var c = s[i];
                if (!char.IsLetterOrDigit(c)
                    && c != '-'
                    && c != '_')
                {
                    s = s.Replace(c, '-');
                }
            }

            if (s.Length > 80)
            {
                throw new Exception(
                    string.Format("Address {0} with configured prefix {1} is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter queue name.", destination, queueNamePrefix));
            }

            return s;
        }
    }
'@