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

- Set up a [Windows Failover Cluster](https://learn.microsoft.com/en-us/windows-server/failover-clustering/create-failover-cluster?pivots=failover-cluster-manager),
  - Make sure to set up clustered storage for the cluster. 
- Install ServiceControl on each node.
- Add each instance as "Generic service" to the cluster using Failover Cluster Manager.
  - Check "Use network name as computer name".
- On the cluster, create all the queues that were created locally for all of the instances of ServiceControl.
- Set up a [Message Queuing role on the cluster](https://learn.microsoft.com/en-us/windows-server/failover-clustering/create-failover-cluster?pivots=failover-cluster-manager#create-clustered-roles-in-failover-cluster-manager)
  - Make sure it uses the clustered storage.

### Database high availability

The internal ServiceControl database (normally a RavenDB database) must be located in a shared storage that is highly available and fault tolerant. Shared storage does not mean a network share, but a cluster storage that allows low latency and exclusive access. 
Access to the data should always be local, although physically that data could be stored on a SAN. When this disk is mounted, ServiceControl must be configured to use that location. 

## ServiceControl configuration

The following steps must be applied to all ServiceControl instances on every node in the cluster.

### Configuration

ServiceControl configuration file (for example, ServiceControl.exe.config) must be modified by defining:

- `DbPath` to define the path to the shared location of the database.
- `Hostname` to reflect `cluster name`.

For example:

```xml
<add key="ServiceControl/DbPath" value="drive:\SomeDir\" />
<add key="ServiceControl/Hostname" value="clusterName" />
```

See [Customizing ServiceControl Configuration](/servicecontrol/servicecontrol-instances/configuration.md) for more information on what each setting means.
