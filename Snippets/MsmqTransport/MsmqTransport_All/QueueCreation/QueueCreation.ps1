
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Messaging

Function Usage
{
# startcode msmq-create-queues-endpoint-usage-powershell
    CreateQueue("myendpoint", [System.Environment]::UserName);
# endcode
# startcode msmq-create-queues-shared-usage-powershell
    CreateQueue("error", [System.Environment]::UserName);
    CreateQueue("audit", [System.Environment]::UserName);
# endcode
}

# startcode msmq-create-queues-for-endpoint-powershell
Function CreateQueuesForEndpoint
{
    param(
        [Parameter(Mandatory=$true)]
        [string] $endpointName,
        [Parameter(Mandatory=$true)]
        [string] $account
    )


    # main queue
    CreateQueue $endpointName $account

    # timeout queue
    CreateQueue ($endpointName + ".timeouts") $account

    # timeout dispatcher queue
    CreateQueue ($endpointName + ".timeoutsdispatcher") $account

    # retries queue
    # TODO: Only required in Versions 5 and below
    CreateQueue ($endpointName + ".retries") $account
}
# endcode

# startcode msmq-create-queues-powershell
Function CreateQueue
{
    param(
        [Parameter(Mandatory=$true)]
        [string] $queueName,
        [Parameter(Mandatory=$true)]
        [string] $account
    )

    $queuePath = '{0}\private$\{1}'-f [System.Environment]::MachineName, $queueName
    if ([System.Messaging.MessageQueue]::Exists($queuePath))
    {
        $messageQueue = New-Object System.Messaging.MessageQueue($queuePath)
        SetPermissionsForQueue $messageQueue $account
        return
    }
    $messageQueue = [System.Messaging.MessageQueue]::Create($queuePath,$true)
    SetPermissionsForQueue $messageQueue $account
}

Function SetPermissionsForQueue
{
    param(
        [Parameter(Mandatory=$true)]
        [System.Messaging.MessageQueue] $queue,
        [Parameter(Mandatory=$true)]
        [string] $account
    )
    $queue.SetPermissions($AdminGroup, "FullControl", "Allow")
    $queue.SetPermissions($EveryoneGroup, "WriteMessage", "Allow")
    $queue.SetPermissions($AnonymousLogon, "WriteMessage", "Allow")

    $queue.SetPermissions($account, "WriteMessage", "Allow")
    $queue.SetPermissions($account, "ReceiveMessage", "Allow")
    $queue.SetPermissions($account, "PeekMessage", "Allow")
}

$AdminGroup=GetGroupName("BuiltinAdministratorsSid")
$EveryoneGroup=GetGroupName("WorldSid")
$AnonymousLogon=GetGroupName("AnonymousSid")

Function GetGroupName
{
    param(
        [Parameter(Mandatory=$true)]
        [System.Security.Principal.WellKnownSidType] $wellKnownSidType
    )

    $account = New-Object System.Security.Principal.SecurityIdentifier($wellKnownSidType, $null)
    return $account.Translate([System.Security.Principal.NTAccount]).ToString()
}

# endcode