# startcode msmq-return-to-source-queue-powershell
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Messaging
Add-Type -AssemblyName System.Transactions

Function ReturnMessageToSourceQueue
{
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $ErrorQueueMachine,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $ErrorQueueName,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $MessageId
    )

    $queuePath = '{0}\private$\{1}'-f $ErrorQueueMachine, $ErrorQueueName

    $propertyFilter = New-Object System.Messaging.MessagePropertyFilter
    $propertyFilter.Body = $true
    $propertyFilter.TimeToBeReceived =$true
    $propertyFilter.Recoverable = $true
    $propertyFilter.Id = $true
    $propertyFilter.ResponseQueue = $true
    $propertyFilter.CorrelationId = $true
    $propertyFilter.Extension = $true
    $propertyFilter.AppSpecific = $true
    $propertyFilter.LookupId = $true

    $errorQueue = New-Object System.Messaging.MessageQueue($queuePath)
    $errorQueue.MessageReadPropertyFilter = $propertyFilter

    $scope = New-Object System.Transactions.TransactionScope
    try
    {
        $transactionType = [System.Messaging.MessageQueueTransactionType]::Automatic
        $message = $errorQueue.ReceiveById($MessageId, [System.TimeSpan]::FromSeconds(5), $transactionType)
        $failedQueuePath = ReadFailedQueueFromHeaders -Message $message
        $failedQueue = New-Object System.Messaging.MessageQueue($failedQueuePath)
        $failedQueue.Send($message, $transactionType)
        $scope.Complete()
    }
    finally {
        $scope.Dispose()
    }
}

Function ReadFailedQueueFromHeaders
{
    param(
        [Parameter(Mandatory=$true)]
        [System.Messaging.Message] $message
    )

    $rawheaders = [System.Text.Encoding]::UTF8.GetString($message.Extension)
    $reader = New-Object System.IO.StringReader($rawheaders)
    $xml = [xml] $reader.ReadToEnd()
    $header =  $xml.ArrayOfHeaderInfo.HeaderInfo | ? Key -eq "NServiceBus.FailedQ" | Select -ExpandProperty Value
    return ('{0}\private$\{1}' -f $header.Split('@')[1], $header.Split('@')[0])
}
# endcode