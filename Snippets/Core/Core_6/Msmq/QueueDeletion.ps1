# startcode msmq-delete-queues-powershell
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Messaging

Function DeleteQueuesForEndpoint
{
    param(
        [Parameter(Mandatory=$true)]
        [string] $endpointName,
		[Parameter(Mandatory=$false)]
		[AllowNull()]
        [string] $instanceId
    )

    #main queue
    DeleteQueue $endpointName

	#instance specific queue
	if ($instanceId) {
		DeleteQueue ($endpointName + "." + $instanceId)
	}

    #timeout queue
    DeleteQueue ($endpointName + ".timeouts")

    #timeout dispatcher queue
    DeleteQueue ($endpointName + ".timeoutsdispatcher")
}

Function DeleteQueue
{
    param(
        [Parameter(Mandatory=$true)]
        [string] $queueName
    )

    $queuePath = '{0}\private$\{1}'-f [System.Environment]::MachineName, $queueName
    if ([System.Messaging.MessageQueue]::Exists($queuePath))
    {
        [System.Messaging.MessageQueue]::Delete($queuePath)
    }
}
Function DeleteAllQueues
{
    foreach ($queue in [System.Messaging.MessageQueue]::GetPrivateQueuesByMachine("."))
    {
        [System.Messaging.MessageQueue]::Delete($queue.Path)
    }
}
#endcode
