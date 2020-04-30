# startcode msmq-nativesend-powershell
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Messaging
Add-Type -AssemblyName System.Transactions

Add-Type @"
    public class HeaderInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
"@

Function CreateHeaders {
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNull()]
        [Hashtable] $entries
    )

    $headerinfos =  New-Object System.Collections.Generic.List[HeaderInfo]
    $entries.Keys | % {
        $headerinfo = New-Object HeaderInfo
        $headerinfo.Key = $_
        $headerinfo.Value = $entries[$_]
        $headerinfos.Add($headerinfo)
    }

    $serializer = New-Object System.Xml.Serialization.XmlSerializer( $headerinfos.GetType() )
    $stream = New-Object System.IO.MemoryStream
    try
    {
        $serializer.Serialize($stream, $headerInfos)
        return $stream.ToArray()
    }
    finally
    {
        $stream.Dispose()
    }
}

Function SendMessage {
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $QueuePath,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $MessageBody,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]

        [Hashtable] $Headers
    )

    $scope = New-Object System.Transactions.TransactionScope
    $queue = New-Object System.Messaging.MessageQueue($QueuePath)
    $message = New-Object System.Messaging.Message

    try
    {
        $msgStream = New-Object System.IO.MemoryStream
        $msgBytes = [System.Text.Encoding]::UTF8.GetBytes($MessageBody)
        $msgStream.Write($msgBytes, 0, $msgBytes.Length)
        $message.BodyStream = $msgStream
        $message.Extension =  CreateHeaders $Headers
        $queue.Send($message, [System.Messaging.MessageQueueTransactionType]::Automatic)
        $scope.Complete()
    }
    finally {
        $scope.Dispose()
        $message.Dispose()
        $queue.Dispose()
    }
}
# endcode