To use [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage, pass `-PersistenceType` and `-DatabaseConnectionString` instead of `-DBPath` and `-DatabaseMaintenancePort`, which apply only to RavenDB. Audit instances do not use external message body storage. The following example uses the same database as the Error instance; the Audit instance's tables go in the `sc_audit` schema by default.

snippet: ps-new-audit-instance-sqlserver
