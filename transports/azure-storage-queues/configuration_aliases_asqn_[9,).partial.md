

### Using aliases for connection strings to storage accounts

Storage account aliases are enforced by default. The alias is mapped to the physical storage account represented by a `QueueServiceClient` or a connection string (for backward compatibility).

> [!NOTE]
> the default alias is an empty string.

See also [Using aliases instead of connection strings](/transports/azure-storage-queues/multi-storageaccount-support.md#nservicebus-routing-with-multiple-storage-accounts-connection-string-aliases) for multi-account support.
