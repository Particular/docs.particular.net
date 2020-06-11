Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Messaging

Function Usage
{
# startcode msmq-create-queues-endpoint-usage-powershell
    # For NServiceBus 6 Endpoints
    CreateQueuesForEndpoint -EndpointName "myendpoint" -Account $env:USERNAME

# For NServiceBus 5 and below Endpoints
    CreateQueuesForEndpoint -EndpointName "myendpoint" -Account $env:USERNAME -IncludeRetries
# endcode

# startcode msmq-create-queues-shared-usage-powershell
    CreateQueue -QueueName "error" -Account $env:USERNAME
    CreateQueue -QueueName "audit" -Account $env:USERNAME
# endcode
}

# startcode msmq-create-queues-for-endpoint-powershell
Function CreateQueuesForEndpoint
{
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $EndpointName,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [ValidateScript({ValidateAccount -Account $_})]
        [string] $Account,

        [Parameter(HelpMessage="Only required for NSB Versions 5 and below")]
        [Switch] $IncludeRetries
    )

    # main queue
    CreateQueue -QueueName $EndpointName -Account $Account

    # timeout queue
    CreateQueue -QueueName "$EndpointName.timeouts" -Account $Account

    # timeout dispatcher queue
    CreateQueue -QueueName "$EndpointName.timeoutsdispatcher" -Account $Account

    # retries queue
    if ($IncludeRetries) {
        CreateQueue -QueueName "$EndpointName.retries" -Account $Account
    }
}
# endcode

# startcode msmq-create-queues-powershell
Function CreateQueue
{
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $QueueName,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [ValidateScript({ValidateAccount -Account $_})]
        [string] $Account
    )

    $queuePath = '{0}\private$\{1}' -f $env:COMPUTERNAME, $QueueName

    if (-Not [System.Messaging.MessageQueue]::Exists($queuePath)) {
        $messageQueue = [System.Messaging.MessageQueue]::Create($queuePath, $true)
        SetDefaultPermissionsForQueue -Queue $messageQueue -Account $Account
    }
    else {
        Write-Warning "$queuepath already exists - no changes were made"
    }
}

Function GetAccountFromWellKnownSid
{
    param(
        [Parameter(Mandatory=$true)]
        [System.Security.Principal.WellKnownSidType] $WellKnownSidType
    )

    $account = New-Object System.Security.Principal.SecurityIdentifier $WellKnownSidType,$null
    return $account.Translate([System.Security.Principal.NTAccount]).ToString()
}

Function ValidateAccount {
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $Account
    )

    # Test Account is valid
    $userAccount =  new-object System.Security.Principal.NTAccount($Account)
    try {
        [void] $userAccount.Translate([System.Security.Principal.SecurityIdentifier])
        return $true
    }
    catch [System.Security.Principal.IdentityNotMappedException] {
        Write-Warning "$account does not resolve to a Windows Account"
        return $false
    }
}

Function SetDefaultPermissionsForQueue
{
    param(
        [Parameter(Mandatory=$true)]
        [System.Messaging.MessageQueue] $Queue,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $Account
    )

    $adminGroup = GetAccountFromWellKnownSid -wellKnownSidType ([System.Security.Principal.WellKnownSidType]::BuiltinAdministratorsSid)
    $Queue.SetPermissions($AdminGroup, "FullControl", "Allow")
    $Queue.SetPermissions($Account, "WriteMessage", "Allow")
    $Queue.SetPermissions($Account, "ReceiveMessage", "Allow")
    $Queue.SetPermissions($Account, "PeekMessage", "Allow")
    $Queue.SetPermissions($Account, "GetQueueProperties", "Allow")
}

# endcode