### Using an Azure Durable Function

Review the [Azure Blob Storage Data Bus cleanup with Azure Functions sample](/samples/databus/blob-storage-databus-cleanup-function/) to see how to use a durable function to clean up attachments.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all data bus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 storage accounts, not on GPv1 accounts.

> [!NOTE]
> The lifecycle policy runs only once a day. The newly configured or updated policy can take up to 24 hours to go into effect. Once the policy is in effect, it could take up to 24 hours for some actions to run for the first time.

#### How lifecycle rules relate to Azure Blob Storage Databus settings

When creating a rule, the blob prefix match filter setting should be set to the value of `databus/` by default. If [the `Container()` or `BasePath()` configuration options](#behavior) have been specified when configuring the data bus the blob prefix match filter setting must be modified to take into account the configured container and/or base path values.

#### Manage the Blob Lifecycle policy via Azure portal

A lifecycle management policy can be set directly on the azure storage account via the Azure portal. Additional information on the configuration, can be found in [the Azure blob lifecycle management policy](https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-policy-configure?source=recommendations&tabs=azure-portal).

#### Manage the Blob Lifecycle policy via the Azure Command-Line Interface (CLI)

The lifecycle management policy can be set in a JSON document via the [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/storage/account/management-policy?view=azure-cli-latest).

```json
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

```bash
az storage account management-policy create --account-name myaccount --policy @policy.json --resource-group myresourcegroup
```
