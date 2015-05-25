Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Messaging
Add-Type -AssemblyName System.Transactions

Function ReturnMessageToSourceQueue {
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
    
    [System.Messaging.MessagePropertyFilter] $messageReadPropertyFilter = New-Object System.Messaging.MessagePropertyFilter
    $messageReadPropertyFilter.Body = $true
    $messageReadPropertyFilter.TimeToBeReceived =$true
    $messageReadPropertyFilter.Recoverable = $true
    $messageReadPropertyFilter.Id = $true
    $messageReadPropertyFilter.ResponseQueue = $true
    $messageReadPropertyFilter.CorrelationId = $true 
    $messageReadPropertyFilter.Extension = $true           
    $messageReadPropertyFilter.AppSpecific = $true         
    $messageReadPropertyFilter.LookupId = $true  
         
    [System.Messaging.MessageQueue] $errorQueue = New-Object System.Messaging.MessageQueue($queuePath)
    $errorQueue.MessageReadPropertyFilter = $messageReadPropertyFilter
    
    [System.Transactions.TransactionScope] $scope = New-Object System.Transactions.TransactionScope
    try
    {
        [System.Messaging.Message] $message = $errorQueue.ReceiveById($MessageId, [System.TimeSpan]::FromSeconds(5), [System.Messaging.MessageQueueTransactionType]::Automatic)
        $failedQueuePath = ReadFailedQueueFromHeaders -Message $message
        [System.Messaging.MessageQueue] $failedQueue = New-Object System.Messaging.MessageQueue($failedQueuePath)
        $failedQueue.Send($message, [System.Messaging.MessageQueueTransactionType]::Automatic)
        $scope.Complete()
    }
    finally {
        $scope.Dispose()
    }
}

Function ReadFailedQueueFromHeaders{
    param(
        [Parameter(Mandatory=$true)]
        [System.Messaging.Message] $message 
    )

    $rawheaders = [System.Text.Encoding]::UTF8.GetString($message.Extension)
    [System.IO.StringReader] $reader = New-Object System.IO.StringReader($rawheaders)
    $xml = [xml] $reader.ReadToEnd()
    $header =  $xml.ArrayOfHeaderInfo.HeaderInfo | ? Key -eq "NServiceBus.FailedQ" | Select -ExpandProperty Value
    return ('{0}\private$\{1}' -f $header.Split('@')[1], $header.Split('@')[0])
}
