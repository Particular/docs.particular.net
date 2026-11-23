---
title: Azure Table Persistence Scripts
summary: Collection of scripts for managing Azure Table Persistence
component: ASP
reviewed: 2026-10-16
related:
 - nservicebus/operations
redirects:
 - nservicebus/azure-storage-persistence/scripting
---

## Removing subscriptions

The following PowerShell script removes all subscriptions of a specific subscriber from the configured Azure storage account. It requires the `Az.Storage` PowerShell module. See the [Azure Table storage PowerShell prerequisites](https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-how-to-use-powershell) for installation instructions.

Copy and paste the script into a PowerShell console and press <kbd>Enter</kbd>:

```ps
function Remove-Subscriptions(
    [string] $accountStorageName,
    [string] $accountStorageKey,
    [string] $subscriptionTableName = 'Subscription',
    [string] $transportAddressToRemove
    )
{
    Import-Module Az.Storage

    # create Context
    $ctx = New-AzStorageContext -StorageAccountName $accountStorageName -StorageAccountKey  $accountStorageKey

    # get table
    $table = Get-AzStorageTable -Name $subscriptionTableName -Context $ctx -ErrorAction Ignore

    # if exists
    if ($table -ne $null) {

        #Create a table query.
        $query = New-Object Microsoft.Azure.Cosmos.Table.TableQuery

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
        $entries = @($table.CloudTable.ExecuteQuery($query))

        Write-Host "Following subscriptions will be removed for " $transportAddressToRemove

        $entries | Format-Table @{ Label = "MessageType"; Expression={$_.PartitionKey}}, @{ Label = "EndpointName"; Expression={$_.Properties["EndpointName"].StringValue }} -AutoSize

        $confirmation = Read-Host "Confirm entry removal? [y/n]"

        if ($confirmation -eq 'y') {
            #Delete entries

            foreach ($entry in $entries) {
                $table.CloudTable.Execute([Microsoft.Azure.Cosmos.Table.TableOperation]::Delete($entry))
            }
        } else {
            Write-Host 'Deletion aborted'
        }
    }
}
```

Then execute the script:

```ps
Remove-Subscriptions -accountStorageName <storageAccountName> -accountStorageKey <storageAccountKey> -subscriptionTableName <subscriptionTable> -transportAddressToRemove <subscriberAddress>
```

Where:

 * `<storageAccountName>` is the name of the storage account containing the subscription storage
 * `<storageAccountKey>` is a key to access the storage account
 * `<subscriptionTable>` is the configured subscription table. By default this is `Subscription`. This parameter is optional.
 * `<subscriberAddress>` is the address of the subscriber which should be removed from the subscription storage

For subscription storage settings, see [Azure Table Persistence configuration](configuration.md#subscription-configuration).
