# startcode sqlserver-powershell-nativesend
Set-StrictMode -Version 2.0

Add-Type -AssemblyName System.Data

Function SendMessage
{
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $ConnectionString,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $Queue,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string] $MessageBody,

        [Parameter(Mandatory=$true)]
        [ValidateNotNull()]
        [HashTable] $Headers
    )

    $UTF8 = [System.Text.Encoding]::UTF8


    $sqlConnection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
    $sqlConnection.Open()

    $sqlCommand = $sqlConnection.CreateCommand()
    $sqlCommand.CommandText =
        "insert into [$Queue] (Id, Recoverable, Headers, Body) values (@Id, @Recoverable, @Headers, @Body)"
    $parameters = $sqlCommand.Parameters
    $parameters.Add("Id", [System.Data.SqlDbType]::UniqueIdentifier).Value = [System.Guid]::NewGuid()
    $serializedHeaders = ConvertTo-Json $Headers
    $parameters.Add("Headers", [System.Data.SqlDbType]::NVarChar).Value = $serializedHeaders
    $parameters.Add("Body", [System.Data.SqlDbType]::VarBinary).Value = $UTF8.GetBytes($MessageBody)
    $parameters.Add("Recoverable", [System.Data.SqlDbType]::Bit).Value = 1
    $sqlCommand.ExecuteNonQuery()

    $sqlConnection.Close()
}
# endcode
Function Usage
{
    # region sqlserver-powershell-nativesend-usage

    SendMessage -ConnectionString "Data Source=.\SqlExpress;Initial Catalog=samples;Integrated Security=True" `
                -Queue "sqlserverNativeSendTests" `
                -MessageBody "{Property:'Value'}" `
                -Headers @{"NServiceBus.EnclosedMessageTypes" = "Operations.SqlServer.NativeSendTests+MessageToSend"}

    # Alternative usage using PowerShell Splatting (e.g. passing params as a hashtable)

    $payload = @{
        ConnectionString = "Data Source=.\SqlExpress;Initial Catalog=samples;Integrated Security=True"
        Queue = "sqlserverNativeSendTests"
        Headers = @{
            "NServiceBus.EnclosedMessageTypes" = "Operations.SqlServer.NativeSendTests+MessageToSend"
        }
        MessageBody = "{'Property':'Value'}"
    }

    SendMessage @payload

    # endregion

}