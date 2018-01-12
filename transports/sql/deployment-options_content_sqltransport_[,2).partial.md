## Default

include: deployment-options-default


## Multi-instance

include: deployment-options-multi-instance

Although the multi-catalog and multi-schema modes are not supported explicitly, they but can be simulated by configuring all connections to refer to a single instance of SQL Server but with different query string properties:
 * `initial catalog` for multi-catalog deployments
 * `queue schema` for multi-schema deployments

The downside of this mode is fact that it requires the Distributed Transaction Coordinator (DTC) even if all catalogs and/or schemas are in the same instance of SQL Server.

NOTE: The `queue schema` is an NServiceBus-specific extension to the SQL Server connection string syntax and can only be used in connection strings used by SQL Server Transport.
 
