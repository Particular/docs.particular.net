#Requires -Modules AWSPowerShell

Set-StrictMode -Version 3.0
Function Usage
{
    # region sqs-powershell-nativesend-usage

    SendMessage -QueueName "samples-sqs-nativeintegration" `
                -MessageBody "{Property:'Value'}" `
                -Headers @{
					"NServiceBus.EnclosedMessageTypes" = "MessageTypeToSend";
					"NServiceBus.MessageId" = "99C7320B-A645-4C74-95E8-857EAB98F4F9"
				}

    # Alternative usage using PowerShell Splatting (e.g. passing params as a hashtable)

    $payload = @{ 
		QueueName = "sqsNativeSendTests"
        Headers = @{
            "NServiceBus.EnclosedMessageTypes" = "MessageTypeToSend";
			"NServiceBus.MessageId" = "99C7320B-A645-4C74-95E8-857EAB98F4F9"
        }
        MessageBody = "{'Property':'Value'}"
    }

    SendMessage @payload

    # endregion

	# region sqs-powershell-nativesend-large-usage

    SendLargeMessage -QueueName "samples-sqs-nativeintegration" `
                -MessageBody "{Property:'Value'}" `
				-S3Prefix "S3Prefix" `
				-BucketName "BucketName" `
                -Headers @{
					"NServiceBus.EnclosedMessageTypes" = "MessageTypeToSend";
					"NServiceBus.MessageId" = "99C7320B-A645-4C74-95E8-857EAB98F4F9"
				}

    # Alternative usage using PowerShell Splatting (e.g. passing params as a hashtable)

    $payload = @{ 
		QueueName = "sqsNativeSendTests"
        Headers = @{
            "NServiceBus.EnclosedMessageTypes" = "MessageTypeToSend";
			"NServiceBus.MessageId" = "99C7320B-A645-4C74-95E8-857EAB98F4F9"
        }
        MessageBody = "{'Property':'Value'}"
		S3Prefix = "S3Prefix"
		BucketName = "BucketName"
    }

    SendLargeMessage @payload

    # endregion

}

# startcode sqs-powershell-nativesend

Function SendMessage
{
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $QueueName,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $MessageBody,

        [Parameter(Mandatory=$true)]
        [ValidateNotNull()]
        [HashTable] $Headers
    )

	$bodyBytes = [System.Text.Encoding]::UTF8.GetBytes($MessageBody)
	$base64Body = [System.Convert]::ToBase64String($bodyBytes)
	[string]$name = [QueueNameHelper]::GetSqsQueueName($QueueName)
	$serializedMessage = @{ Headers = $Headers; Body =  $base64Body } | ConvertTo-Json
	$queueUrl = Get-SQSQueueUrl $name
	Send-SQSMessage -QueueUrl $queueUrl -MessageBody $serializedMessage -Force
}
# endcode

# startcode sqs-powershell-nativesend-large

Function SendLargeMessage
{
	param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $QueueName,

		[Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $S3Prefix,

		[Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $BucketName,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $MessageBody,

        [Parameter(Mandatory=$true)]
        [ValidateNotNull()]
        [HashTable] $Headers
    )
	
	$key = "$($s3Prefix)/$($Headers['NServiceBus.MessageId'])"
    Write-S3Object -BucketName $BucketName -Key $key -Content $MessageBody -Force
	[string]$name = [QueueNameHelper]::GetSqsQueueName($QueueName)
	$serializedMessage = @{ Headers = $Headers; Body =  [System.String]::Empty; S3BodyKey = $key } | ConvertTo-Json
	$queueUrl = Get-SQSQueueUrl $name
	Send-SQSMessage -QueueUrl $queueUrl -MessageBody $serializedMessage -Force
}
# endcode

# startcode sqs-powershell-queue-name-helper
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
# endcode