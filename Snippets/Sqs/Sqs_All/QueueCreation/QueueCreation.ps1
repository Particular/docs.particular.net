#Requires -Modules AWSPowerShell

Set-StrictMode -Version 3.0

Function Usage
{
# startcode sqs-create-queues-endpoint-usage-powershell
# For NServiceBus 6 Endpoints
CreateQueuesForEndpoint -EndpointName "myendpoint" -MaxTimeToLive "2:0:0"

# For NServiceBus 5 and below Endpoints
CreateQueuesForEndpoint -EndpointName "myendpoint" -MaxTimeToLive "2:0:0" -IncludeRetries
# endcode

# startcode sqs-create-queues-shared-usage-powershell
CreateQueue -QueueName "error" -MaxTimeToLive "2:0:0"
CreateQueue -QueueName "audit" -MaxTimeToLive "2:0:0"
# endcode
}

# startcode sqs-create-queues-for-endpoint-powershell
Function CreateQueuesForEndpoint
{
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $EndpointName,

[Parameter(Mandatory=$false)]
[ValidateNotNullOrEmpty()]
[ValidateScript({ValidateMaxTimeToLive -MaxTimeToLive $_})]
[string] $MaxTimeToLive = "4:0:0:0",

[Parameter(HelpMessage="Only required for NSB Versions 5 and below")]
[Switch] $IncludeRetries
)

# main queue
CreateQueue -QueueName $EndpointName -MaxTimeToLive $MaxTimeToLive

# timeout queue
CreateQueue -QueueName "$EndpointName.timeouts" -MaxTimeToLive $MaxTimeToLive

# timeout dispatcher queue
CreateQueue -QueueName "$EndpointName.timeoutsdispatcher" -MaxTimeToLive $MaxTimeToLive

# retries queue
if ($IncludeRetries) {
CreateQueue -QueueName "$EndpointName.retries" -MaxTimeToLive $MaxTimeToLive
}
}
# endcode

# startcode sqs-create-queues-powershell
Function CreateQueue
{
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $QueueName,

[Parameter(Mandatory=$false)]
[ValidateNotNullOrEmpty()]
[ValidateScript({ValidateMaxTimeToLive -MaxTimeToLive $_})]
[string] $MaxTimeToLive = "4:0:0:0"
)
$timeToLive = [System.Convert]::ToInt32([System.TimeSpan]::Parse($MaxTimeToLive, [System.Globalization.CultureInfo]::InvariantCulture).TotalSeconds).ToString()
[string]$name = [QueueNameHelper]::GetSqsQueueName($QueueName)
New-SQSQueue -QueueName $name -Attributes @{"MessageRetentionPeriod" = $timeToLive;} -Force
}

Function ValidateMaxTimeToLive {
param(
[Parameter(Mandatory=$true)]
[ValidateNotNullOrEmpty()]
[string] $MaxTimeToLive
)

# Test MaxTimeToLive is valid
  
try {
$maxTimeToLive = [System.TimeSpan]::Parse($MaxTimeToLive, [System.Globalization.CultureInfo]::InvariantCulture)
return $true
}
catch [System.FormatException] {
Write-Warning "$MaxTimeToLive is not a valid TimeSpan"
return $false
}
}

# endcode

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