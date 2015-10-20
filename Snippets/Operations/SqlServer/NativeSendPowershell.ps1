# startcode sqlserver-powershell-nativesend
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Data

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

	$sqlCommand.Parameters.Add("Id", [System.Data.SqlDbType]::UniqueIdentifier).Value = [System.Guid]::NewGuid()
	$serializeHeaders = ConvertTo-Json $headers;
	$sqlCommand.Parameters.Add("Headers", [System.Data.SqlDbType]::VarChar).Value = $serializeHeaders

	$UTF8 = [System.Text.Encoding]::UTF8

	$sqlCommand.Parameters.Add("Body", [System.Data.SqlDbType]::VarBinary).Value = $UTF8.GetBytes("$messageBody")
	$sqlCommand.Parameters.Add("Recoverable", [System.Data.SqlDbType]::Bit).Value = 1

	$sqlCommand.ExecuteNonQuery()

	$sqlConnection.Close()
}
# endcode
Function Usage
{

	# startcode sqlserver-powershell-nativesend-usage

	SendMessage "Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True" "sqlserverNativeSendTests" "{'Property':'Value'}" @{"NServiceBus.EnclosedMessageTypes" = "Operations.SqlServer.NativeSendTests+MessageToSend"}

	# endcode

}