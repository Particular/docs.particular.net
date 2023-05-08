### Using an Azure Durable Function

Review the [sample](/samples/azure/blob-storage-databus-cleanup-function/) to see how to use a durable function to clean up attachments.

### Using the Blob Lifecycle Management policy

Attachment blobs can be cleaned up using the [Blob Storage Lifecycle feature](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts). This method allows configuring a single policy for all data bus-related blobs. Those blobs can be either deleted or archived. The policy does not require custom code and is deployed directly to the storage account. This feature can only be used on GPv2 and Blob storage accounts, not on GPv1 accounts.
The policy can be set on the storage account via the Azure portal or through a powershell command.

#### Manage the Blob Lifecycle policy via Azure portal
Click  on the Lifecycle management option in the azurestorage. As a lifecycle management policy is a collection of rules, select add a rule option.

![lifecyclemanagement_rule](https://user-images.githubusercontent.com/88632084/236739079-44975ff4-5f83-49ed-bbc9-6fdf3cc08753.png)

Specify a rule name, its scope and the blob type it should be applied to.
![rule_details](https://user-images.githubusercontent.com/88632084/236739377-ed7dff37-2a6b-4954-91ab-a4f72c4d0c04.png)

Rule conditions can be specified through if/then statements.
![rule_baseblob](https://user-images.githubusercontent.com/88632084/236739541-188d4d68-e181-4a79-953b-227a5c91c634.png)

Optional filters can be applied to the blobs inside a container
![rule_filter](https://user-images.githubusercontent.com/88632084/236739636-900f662b-ca76-4754-b09a-bb5f7072ee2d.png)

Clicking on add creates the new policy on the storage account.
![newrule_listview](https://user-images.githubusercontent.com/88632084/236739750-518bf147-bcee-40fa-9d5c-148dd42b1c9b.png)

The polices can also be edited through the code view
![newrule_codeview](https://user-images.githubusercontent.com/88632084/236739819-163143b1-b23b-47bb-8123-26d0a8ef9e0d.png)

Th lifecycle policy runs only once a day. The newly configured or updated policy can take up to 24 hours to go into effect. Once the policy is in effect, it could take up to 24 hours for some actions to run for the first time.

For additional information on policy configuration, please (click here](https://learn.microsoft.com/en-us/azure/storage/blobs/lifecycle-management-policy-configure?source=recommendations&tabs=azure-portal)

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
The data policy rules associated with the specified storage account can be created as follows. 
```
az storage account management-policy create --account-name myaccount --policy @policy.json --resource-group myresourcegroup
```
For additional commands on the policy management please click [here](https://learn.microsoft.com/en-us/cli/azure/storage/account/management-policy?view=azure-cli-latest)
