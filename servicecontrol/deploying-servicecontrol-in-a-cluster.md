---
title: Deploying ServiceControl to a Cluster
summary: A guide to deploying ServiceControl on a Windows failover cluster
related:
- servicecontrol/troubleshooting
reviewed: 2025-10-22
---

The following procedure is a high-level guide on how to deploy ServiceControl onto a fault-tolerant cluster using Windows Failover Clustering.

> [!NOTE]
> This guide assumes that MSMQ is the underlying transport. Other transports work as long as these are deployed on a different machine. In that case, skip the MSMQ-specific steps.

> [!NOTE]
> ServiceControl only supports active/passive clusters.

## Infrastructure setup

1. Create a [Windows Failover Cluster](https://learn.microsoft.com/en-us/windows-server/failover-clustering/create-failover-cluster?pivots=failover-cluster-manager),
1. Create a [Message Queuing role on the cluster](https://learn.microsoft.com/en-us/windows-server/failover-clustering/create-failover-cluster?pivots=failover-cluster-manager#create-clustered-roles-in-failover-cluster-manager)
1. Install ServiceControl on each node of the cluster.
1. Add the ServiceControl Windows Service as a Generic Service resource to the MSMQ cluster role.
    1. Ensure it depends on the MSMQ role and the MSMQ network name
    1. After setting up the dependencies, check the "Use Network Name as computer name" option

Before bringing the service resource online, all of the [required queues](/servicecontrol/queues.md#queue-setup) must be created and given the appropriate permissions manually in the cluster.

### Database high availability

The internal ServiceControl RavenDB database must be located in a shared storage that is highly available and fault tolerant. Shared storage does not mean a network share, but a cluster storage that allows low latency and exclusive access.
Access to the data should always be local, although physically that data could be stored on a SAN. When this disk is mounted, ServiceControl must be configured to use that location.

## ServiceControl configuration

The following settings must be applied to all ServiceControl instances on every node in the cluster.

### Configuration

The ServiceControl configuration must be customized by changing the following settings:

- The database path needs to set to the path to the shared location of the database.
  - `ServiceControl/DBPath` for error instances
  - `ServiceControl.Audit/DBPath` for audit instances
- The host name needs to be set to the MSMQ network name
  - `ServiceControl/HostName` for error instances
  - `ServiceControl.Audit/HostName` for audit instances
  - `Monitoring/HttpHostName` for monitoring instances
- The `ServiceControl/RemoteInstances` setting should use the MSMQ network name to refer to the audit instance

See [Customizing ServiceControl Configuration](/servicecontrol/servicecontrol-instances/configuration.md) for more information on each setting.