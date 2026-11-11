> [!NOTE]
> In production environments, review the [hardware considerations](/servicecontrol/servicecontrol-instances/hardware.md) before setting up a machine for ServiceControl. What the machine needs depends on where the instance stores its data: an Error or Audit instance that runs embedded RavenDB needs a machine sized as a database server, while one that uses SQL Server, PostgreSQL, or an external RavenDB server does not. Monitoring instances do not need a database.

For production systems, it is recommended to limit the number of instances per machine to one of each [type](/servicecontrol/#servicecontrol-instance-types). The ability to add multiple instances *of the same type on a single machine* is primarily intended to assist development and test environments.

For RavenDB storage, see also [capacity planning for RavenDB storage](/servicecontrol/capacity-and-planning.md).
