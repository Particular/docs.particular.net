# startcode Remove-Subscriptions

function Remove-Subscriptions(
    [string] $accountStorageName,
    [string] $accountStorageKey,
    [string] $subscriptionTableName = 'Subscription',
    [string] $transportAddressToRemove
    )
{
    # create Context
    $ctx = New-AzureStorageContext -StorageAccountName $accountStorageName -StorageAccountKey  $accountStorageKey

    # get table
    $table = Get-AzureStorageTable -Name $subscriptionTableName -Context $ctx -ErrorAction Ignore

    # if exists
    if ($table -ne $null) {

        #Create a table query.
        $query = New-Object Microsoft.WindowsAzure.Storage.Table.TableQuery

        #Define columns to select.
        $list = New-Object System.Collections.Generic.List[string]
        $list.Add("RowKey")
        $list.Add("PartitionKey")
        $list.Add("EndpointName")

        #Calculate RowKey based on addressToRemove
        $asciiEncoded = [System.Text.Encoding]::ASCII.GetBytes($transportAddressToRemove)
        $base64Encoded = [System.Convert]::ToBase64String($asciiEncoded)

        #Set query details.
        $query.FilterString = "RowKey eq '" + $base64Encoded + "'"
        $query.SelectColumns = $list

        #Execute the query.
        $entries = $table.CloudTable.ExecuteQuery($query)

        Write-Host "Following subscriptions will be removed for " $transportAddressToRemove

        $entries | Format-Table @{ Label = "MessageType"; Expression={$_.PartitionKey}}, @{ Label = "EndpointName"; Expression={$_.Properties["EndpointName"].StringValue }} -AutoSize

        $confirmation = Read-Host "Confirm entry removal? [y/n]"

        if ($confirmation -eq 'y') {
            #Delete entries

            foreach ($entry in $entries) {
                $table.CloudTable.Execute([Microsoft.WindowsAzure.Storage.Table.TableOperation]::Delete($entry))
            }
        } else {
            Write-Host 'Deletion aborted'
        }
    }
}
# endcode