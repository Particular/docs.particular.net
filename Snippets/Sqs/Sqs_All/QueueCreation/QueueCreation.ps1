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
CreateQueue -QueueName "$EndpointName.Timeouts" -MaxTimeToLive $MaxTimeToLive

# timeout dispatcher queue
CreateQueue -QueueName "$EndpointName.TimeoutsDispatcher" -MaxTimeToLive $MaxTimeToLive

# retries queue
if ($IncludeRetries) {
CreateQueue -QueueName "$EndpointName.Retries" -MaxTimeToLive $MaxTimeToLive
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
	using System.Linq;

    public static class QueueNameHelper
    {
        public static string GetSqsQueueName(string destination, string queueNamePrefix = null, bool preTruncateQueueNames = false)
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException("destination");
            }


            var s = queueNamePrefix + destination;

            if (preTruncateQueueNames && s.Length > 80)
            {
                if (string.IsNullOrWhiteSpace(queueNamePrefix))
                {
                    throw new ArgumentNullException("queueNamePrefix");
                }

                var charsToTake = 80 - queueNamePrefix.Length;
                s = queueNamePrefix +
                    new string(s.Reverse().Take(charsToTake).Reverse().ToArray());
            }

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