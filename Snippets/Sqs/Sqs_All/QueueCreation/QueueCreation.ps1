﻿#Requires -Modules AWSPowerShell

Set-StrictMode -Version 3.0

Function Usage
{
# startcode sqs-create-queues-endpoint-usage-powershell
# For NServiceBus 6 Endpoints
CreateQueuesForEndpoint -EndpointName "myendpoint" -QueueNamePrefix "PROD" -MaxTimeToLive "2:0:0"

# For NServiceBus 5 and below Endpoints
CreateQueuesForEndpoint -EndpointName "myendpoint" -QueueNamePrefix "PROD" -MaxTimeToLive "2:0:0" -IncludeRetries
# endcode

# startcode sqs-create-queues-shared-usage-powershell
CreateQueue -QueueName "error" -QueueNamePrefix "PROD" -MaxTimeToLive "2:0:0"
CreateQueue -QueueName "audit" -QueueNamePrefix "PROD" -MaxTimeToLive "2:0:0"
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

[Parameter(Mandatory=$false)]
[string] $QueueNamePrefix,

[Parameter(HelpMessage="Only required for NSB Versions 5 and below")]
[Switch] $IncludeRetries
)

# timeout dispatcher queue
# This queue is created first because it has the longest name. 
# If the endpointName and queueNamePrefix are too long this call will throw and no queues will be created. 
# In this event, a shorter value for endpointName or queueNamePrefix should be used.
CreateQueue -QueueName "$EndpointName.TimeoutsDispatcher" -QueueNamePrefix $QueueNamePrefix -MaxTimeToLive $MaxTimeToLive

# main queue
CreateQueue -QueueName $EndpointName -QueueNamePrefix $QueueNamePrefix -MaxTimeToLive $MaxTimeToLive

# timeout queue
CreateQueue -QueueName "$EndpointName.Timeouts" -QueueNamePrefix $QueueNamePrefix -MaxTimeToLive $MaxTimeToLive

# retries queue
if ($IncludeRetries) {
CreateQueue -QueueName "$EndpointName.Retries" -QueueNamePrefix $QueueNamePrefix -MaxTimeToLive $MaxTimeToLive
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
[string] $QueueNamePrefix,

[Parameter(Mandatory=$false)]
[ValidateNotNullOrEmpty()]
[ValidateScript({ValidateMaxTimeToLive -MaxTimeToLive $_})]
[string] $MaxTimeToLive = "4:0:0:0"
)
$timeToLive = [System.Convert]::ToInt32([System.TimeSpan]::Parse($MaxTimeToLive, [System.Globalization.CultureInfo]::InvariantCulture).TotalSeconds).ToString()
[string]$name = [QueueNameHelper]::GetSqsQueueName($QueueName, $QueueNamePrefix)
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
                throw new ArgumentException(
                    string.Format("Address {0} with configured prefix {1} is longer than 80 characters and therefore cannot be used to create an SQS queue. Use a shorter endpoint name or a shorter queue name prefix.", destination, queueNamePrefix));
            }

            return s;
        }
    }
'@