### Using aliases instead of connection strings

In order to prevent accidentally leaking connection string values, it is recommended to use aliases instead of raw connection strings. When applied, raw connection string values are replaced with registered aliases removing the possibility of leaking a connection string value. The concept of using aliases for connection strings to storage accounts has been introduced in Version 7. When using a single account, aliasing connection string is limited to just enabling it. When multiple accounts are used, an alias has to be registered for each storage account.

Consider the following example:

 * Two endpoints using different accounts `account_A` and `account_B` for their input queues.
 * The `account_A` endpoint uses account with the following connection string `account_A_connection_string`.
 * The `account_B` endpoint uses account with the following connection string `account_B_connection_string`.
 * Every endpoint sends/replies to messages to the other using `@` notation.
  * `queue@account_A` is a `queue` of the `account_A` endpoint where `account_B` endpoint sends messages to.
  * `queue@account_B` is a `queue` of the `account_B` endpoint where `account_A` endpoint sends messages to.

To enable sending from `account_A` to `account_B`, following configuration has to be applied in the `account_A` endpoint

snippet: AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

To enable sending from `account_B` to `account_A`, following configuration has to be applied in the `account_B` endpoint

snippet: AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings2

Aliases can be provided for both the endpoint's connection string as well as other accounts' connection strings. This enables using `@` notation for destination addresses `queue_name@accountName`.

snippet: storage_account_routing_send_options_alias

NOTE: The examples above use different values for the default account aliases. Using the same name, such as `default`, to represent different storage accounts in different endpoints is highly discouraged as it introduces ambiguity in resolving addresses like `queue@default` and may cause issues when e.g. replying. In that case an address is interpreted as a reply address, the name `default` will point to a different connection string.

NOTE: This feature is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/) is required in order to leverage both.
