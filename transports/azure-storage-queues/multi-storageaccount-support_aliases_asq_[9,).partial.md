### Aliases instead of connection strings

To avoid connection strings leaking, aliases are always used, using an empty string as the default.
Therefore, when multiple accounts are used, an alias has to be registered for each storage account.

To enable sending from `account_A` to `account_B`, the following configuration needs to be applied in the `account_A` endpoint:

snippet: AzureStorageQueueUseMultipleAccountAliasesInsteadOfConnectionStrings1

Aliases can be provided for both the endpoint's connection strings as well as other accounts' connection strings. This enables using the `@` notation for destination addresses like `queue_name@accountAlias`.

snippet: storage_account_routing_send_options_alias

NOTE: The examples above use different values for the default account aliases. Using the same name, such as `default`, to represent different storage accounts in different endpoints is highly discouraged as it introduces ambiguity in resolving addresses like `queue@default` and may cause issues when e.g. replying. In that case an address is interpreted as a reply address, the name `default` will point to a different connection string.

NOTE: This feature is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/) is required in order to leverage both.
