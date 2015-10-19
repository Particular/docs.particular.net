# startcode sqlserver-powershell-nativesend
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Data

Function Usage
{

		# region sqlserver-powershell-nativesend-usage

        SendMessage "Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True" "Samples.SqlServer.NativeIntegration" "{'Property':'PropertyValue'}" @{"NServiceBus.EnclosedMessageTypes" = "MessageToSend"}

        # endregion
       
}

Function SendMessage
{
    param(
        [Parameter(Mandatory=$true)]
        [string] $connectionString,
        [Parameter(Mandatory=$true)]
        [string] $queue,
		[Parameter(Mandatory=$true)]
        [string] $messageBody,
		[Parameter(Mandatory=$true)]
        [object] $headers
    )

	$sqlConnection = new-object System.Data.SqlClient.SqlConnection $connectionString

	$sqlConnection.Open()

	$sqlCommand = $sqlConnection.CreateCommand()

	$sqlCommand.CommandText = "INSERT INTO [" + $queue + "] ([Id],[Recoverable],[Headers],[Body]) VALUES (@Id, @Recoverable, @Headers, @Body)";

	$sqlParameters = $sqlCommand.Parameters;
	
	$sqlParameters.Add("Id", [System.Data.SqlDbType]::UniqueIdentifier).Value = [System.Guid]::NewGuid()
	$serializeHeaders = ConvertTo-Json $headers;
	$sqlParameters.Add("Headers", [System.Data.SqlDbType]::VarChar).Value = $serializeHeaders

    $UTF8 = [System.Text.Encoding]::UTF8

	$sqlParameters.Add("Body", [System.Data.SqlDbType]::VarBinary).Value = $UTF8.GetBytes("$messageBody")
	$sqlParameters.Add("Recoverable", [System.Data.SqlDbType]::Bit).Value = true

	$sqlCommand.ExecuteNonQuery()

	$sqlConnection.Close()
}