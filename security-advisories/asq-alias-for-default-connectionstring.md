---
title: Security Advisory 2018-01-19
summary: Azure Storage Queues vulnerability
reviewed: 2019-10-18
---

This advisory discloses a security vulnerability that has been found in Azure Storage Queues version 7 and fixed in the released hotfixes 7.5.3 and 7.4.3.

 * All endpoints should be upgraded to the latest version of the Azure Storage Queues package to fix this vulnerability if:
   * using the [Azure Storage Queues](/transports/azure-storage-queues) transport version 7
   * using a single storage account
   * using [an alias to secure connection strings](/transports/azure-storage-queues/configuration.md#connection-strings-using-aliases-for-connection-strings-to-storage-accounts)

This advisory affects all versions of the Azure Storage Queues version 7.


## Vulnerability: default connection string is sent over the wire

A vulnerability has been fixed to remove the raw value of the default connection string from an Azure Storage Queues message when it should not be included (i.e., when the `UseAccountAliasesInsteadOfConnectionStrings()` API is used).


## Impact

A message logged could expose the Azure Storage connection string used by the transport. This would allow an attacker with access to log files to gain access to the Azure Storage account.


## Exploitability

The exploitation of this vulnerability would require a message to be logged and logs to fall into the hands of the attacker.


## Affected versions

Versions 7.5.0-7.5.2 and 7.4.0-7.4.2 of Azure Storage Queues transport are affected by this vulnerability. The fix is tracked in the following issues:

- 7.5.x: https://github.com/Particular/NServiceBus.AzureStorageQueues/issues/309
- 7.4.x: https://github.com/Particular/NServiceBus.AzureStorageQueues/issues/312


## Risk Mitigation

If it is not possible to upgrade all endpoints that are using the affected version of the transport, the following can be used as a **risk mitigation**:

 * Verify logs do not contain the connection string
 * Validate that there is no external access to log files


## Fix

This vulnerability can be fixed by upgrading the transport package. Upgrades should be performed as follows:


### Version 7.4.x users should upgrade to Version 7.4.3 or higher.

**Option 1: Full update**

Update the NuGet package using `Update-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues`, re-compile the endpoint/application, and redeploy the endpoint/application.

**Option 2: In-place update**

Update the NuGet package using `Update-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues`, stop affected endpoints, copy the new `NServiceBus.Azure.Transports.WindowsAzureStorageQueues.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.


### Version 7.5.x users should upgrade to Version 7.5.3 or higher.

**Option 1 Full update**

Update the NuGet package using `Update-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues`, re-compile the endpoint/application, and redeploy the endpoint/application.

**Option 2  In-place update**

Update the NuGet package using `Update-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues`, stop affected endpoints, copy the new `NServiceBus.Azure.Transports.WindowsAzureStorageQueues.dll` assembly, overwriting the one(s) currently deployed for the endpoints, and restart the affected endpoints.


## Contact Info

If there are any questions or concerns regarding this advisory, send an email to [security@particular.net](mailto://security@particular.net).