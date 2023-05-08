### Using an Azure Durable Function

Review the [sample](/samples/azure/blob-storage-databus-cleanup-function/) to see how to use a durable function to clean up attachments.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all data bus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 and Blob storage accounts, not on GPv1 accounts.
The policy can be set on the storage account via the Azure portal or through a powershell command.

#### Manage the Blob Lifecycle policy via Azure portal

#### Manage the Blob Lifecycle policy via powershell command


A lifecycle management policy is a collection of rules in a JSON document. 
```
{
  "rules": [
    {
      "enabled": true,
      "name": "sample-databus-rule",
      "type": "Lifecycle",
      "definition": {
        "actions": {
          "version": {
            "delete": {
              "daysAfterCreationGreaterThan": 90
            }
          },
          "baseBlob": {
            "tierToCool": {
              "daysAfterModificationGreaterThan": 30
            },
            "tierToArchive": {
              "daysAfterModificationGreaterThan": 90,
              "daysAfterLastTierChangeGreaterThan": 7
            },
            "delete": {
              "daysAfterModificationGreaterThan": 2555
            }
          }
        },
        "filters": {
          "blobTypes": [
            "blockBlob"
          ],
          "prefixMatch": [
            "sample-databus-container/"
          ]
        }
      }
    }
  ]
}
```
The data policy rules associated with the specified storage account can be created as follows:
```

az storage account management-policy create --account-name myaccount --policy @policy.json --resource-group myresourcegroup

```