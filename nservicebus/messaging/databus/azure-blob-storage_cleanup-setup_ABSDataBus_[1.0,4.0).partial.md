### Using the built-in clean-up method

Specify a value for the `TimeToBeReceived` property. For more details on how to specify this, see [Discarding Old Messages](/nservicebus/messaging/discard-old-messages.md).

WARN: The built-in method uses continuous blob scanning which can add to the cost of the storage operations. It is **not** recommended for multiple endpoints that are scaled out. If this method is not used, be sure to disable the built-in cleanup by setting the `CleanupInterval` to `0`. In versions 3 and above built-in cleanup is disabled by default.

### Using an Azure Durable Function

Review the [sample](/samples/azure/blob-storage-databus-cleanup-function/) to see how to use a durable function to clean up attachments.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all data bus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 and Blob storage accounts, not on GPv1 accounts.

The policy can be set on the storage account via the Azure portal or through a powershell command. The lifecycle policy runs only once a day. The newly configured or updated policy can take up to 24 hours to go into effect. Once the policy is in effect, it could take up to 24 hours for some actions to run for the first time.


#### Manage the Blob Lifecycle policy via Azure portal

A lifecycle management policy is a collection of rules and can be set directly on the azure storage account via the portal.

For additional information on policy configuration, please [click here](https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-policy-configure?source=recommendations&tabs=azure-portal)

#### Manage the Blob Lifecycle policy via powershell command

The collection of rules of  lifecycle management policy can be set in a JSON document. 
```
{
  "rules": [
    {
      "enabled": true,
      "name": "delete-databus-files",
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
            "databus/"
          ]
        }
      }
    }
  ]
}
```
The data policy rules associated with the specified storage account can be created as follows. 
```
az storage account management-policy create --account-name myaccount --policy @policy.json --resource-group myresourcegroup
```
For additional commands on the policy management please click [here](https://learn.microsoft.com/en-us/cli/azure/storage/account/management-policy?view=azure-cli-latest)
