## Default

include: deployment-options-default

## Multi-schema

include: deployment-options-multi-schema

NOTE: The `queue schema` NServiceBus-specific query string property is no longer supported in Version 3 and above. Code-based addressing configuration has to be used instead.

## Multi-catalog

Starting from Version 3.1.1 SQL Server transport supports the multi-catalog mode.

include: deployment-options-multi-catalog

## Multi-instance

WARNING: The multi-instance option is deprecated and has been removed in Version 4. In order to enable it, use `EnableLegacyMultiInstanceMode`.

include: deployment-options-multi-instance

In Versions 3.1.x SQL Server Transport does not support the multi-catalog mode explicitly. This mode can be simulated by configuring all connections to refer to a single instance of SQL Server but with different `initial catalog` query string property.
