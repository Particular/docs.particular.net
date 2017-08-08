#Requires -Modules AWSPowerShell

Set-StrictMode -Version 3.0

Function Usage
{
# startcode sqs-delete-queues-endpoint-usage-powershell
# For NServiceBus 6 Endpoints
DeleteQueuesForEndpoint -EndpointName "myendpoint"

# For NServiceBus 5 and below Endpoints
DeleteQueuesForEndpoint -EndpointName "myendpoint" -IncludeRetries
# endcode

# startcode sqs-delete-queues-shared-usage-powershell
DeleteQueue -QueueName "error"
DeleteQueue -QueueName "audit"
# endcode
}

# startcode sqs-delete-queues-for-endpoint-powershell
Function DeleteQueuesForEndpoint
{
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $EndpointName,

[Parameter(HelpMessage="Only required for NSB Versions 5 and below")]
[Switch] $IncludeRetries
)

# main queue
DeleteQueue -QueueName $EndpointName

# timeout queue
DeleteQueue -QueueName "$EndpointName.timeouts"

# timeout dispatcher queue
DeleteQueue -QueueName "$EndpointName.timeoutsdispatcher"

# retries queue
if ($IncludeRetries) {
DeleteQueue -QueueName "$EndpointName.retries"
}
}
# endcode

# startcode sqs-delete-queues-powershell
Function DeleteQueue
{
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $QueueName
)
[string]$name = [QueueNameHelper]::GetSqsQueueName($QueueName)
$queueUrl = Get-SQSQueueUrl $name
Remove-SQSQueue -QueueUrl $queueUrl -Force
}

# endcode

Function DeleteAllQueues
{
param(
)
Get-SQSQueue | Remove-SQSQueue -Force
}

Add-Type @'
    using System;

    public static class QueueNameHelper
    {
        public static string GetSqsQueueName(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException("destination", "destination");
            }

            // SQS queue names can only have alphanumeric characters, hyphens and underscores.
            // Any other characters will be replaced with a hyphen.
            for (var i = 0; i < destination.Length; ++i)
            {
                var c = destination[i];
                if (!char.IsLetterOrDigit(c)
                    && c != '-'
                    && c != '_')
                {
                    destination = destination.Replace(c, '-');
                }
            }

            if (destination.Length > 80)
            {
                throw new Exception(
                    "Address is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter queue name.");
            }

            return destination;
        }
    }
'@