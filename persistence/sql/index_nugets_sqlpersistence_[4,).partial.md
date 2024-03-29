The SQL Persister requires the [**NServiceBus.Persistence.Sql**](https://www.nuget.org/packages/NServiceBus.Persistence.Sql) package, which contains:

 * APIs for manipulating `EndpointConfiguration`.
 * Runtime implementations of saga, timeouts, subscriptions, and outbox persisters.
 * Attributes used to define compile-time configuration settings.
 * **[Version 4.3+]** An MSBuild target that generates the required SQL installation scripts at compile time.
 * Optionally runs SQL installation scripts at endpoint startup for development purposes. See [Installer Workflow](installer-workflow.md).

WARNING: In version 4.2 and below, The [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) package must also be referenced by any project that requires generating SQL installation scripts. This includes any project that contains saga classes or hosts an NServiceBus endpoint.

### Other packages

* [NServiceBus.Persistence.Sql.MsBuild](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.MsBuild) - In version 4.2 and below, this package must also be referenced to generate scripts. In version 4.3 and above, this package is deprecated, as the generation capability is built in to the main package.
* [NServiceBus.Persistence.Sql.ScriptBuilder](https://www.nuget.org/packages/NServiceBus.Persistence.Sql.ScriptBuilder) - This package contains the APIs that enable the generation of SQL installation scripts using code outside of a compile context. It is not intended for general usage, and in future releases, the API may evolve in ways that do not follow [semantic versioning](/nservicebus/upgrades/release-policy.md#semantic-versioning).
