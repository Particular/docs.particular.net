---
title: Azure Storage Persistence Scripts
summary: Collection of scripts for managing Azure Storage Persistence
reviewed: 2016-09-29
component: ASP
tags:
- Azure
- Persistence
---


## Removing Subscriptions

The following PowerShell script removes all subscriptions of a specific subscriber from the configured Azure storage account. Copy and paste the script to a PowerShell and hit Enter:

snippet: Remove-Subscriptions

Then execute the script:

```ps
Remove-Subscriptions -accountStorageName <storageAccountName> -accountStorageKey <storageAccountKey> -subscriptionTableName <subscriptionTable> -transportAddressToRemove <subscriberAddress>
```

Where:

 * `<storageAccountName>` is the name of the storage account containing the subscription storage.
 * `<storageAccountKey>` is a key to access the storage account.
 * `<subscriptionTable>` is the configured subscription table. By default this is `Subscription`. This parameter is optional.
 * `<distributorAddress>` is the address of the subscriber which should be removed from the subscription storage.