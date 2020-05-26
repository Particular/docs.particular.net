---
title: Deploying ServiceControl to a Cluster
summary: A guide to deploying ServiceControl on a Windows cluster
related:
- servicecontrol/troubleshooting
reviewed: 2019-10-24
---

NOTE: Clustering might not be required as cloud hosting and enterprise virtualization layers provide high availability and data redundancy features.

The following procedure is a high level guide on how to deploy ServiceControl onto a fault-tolerant cluster using Windows Failover Clustering.

NOTE: This guide assumes that MSMQ is the underlying transport.


## Basic setup

* Set up a failover (active/passive) Windows cluster:
  * [Windows Server 2008](https://blogs.msdn.microsoft.com/clustering/2008/01/18/creating-a-cluster-in-windows-server-2008/)
  * [Windows Server 2012 R2, Windows Server 2012, Windows Server 2016](https://docs.microsoft.com/en-us/windows-server/failover-clustering/create-failover-cluster)
* Install ServiceControl on each node, adding it as a "generic service" using the cluster manager. This means that ServiceControl will failover automatically with the cluster.
* Set up an MSMQ cluster group. A cluster group is a group of resources that have a unique DNS name and can be addressed externally like a computer.
* Add the ServiceControl generic clustered service to the MSMQ cluster group:
  * Ensure it depends on MSMQ and the MSMQ network name
  * Check "use network name as computer name" in the service configuration

Once set up, ServiceControl queues will be available on the cluster. The server name will be the MSMQ network name, not to be confused with the cluster name.

More information is available on [Message Queuing in Server Clusters](https://technet.microsoft.com/en-us/library/cc753575.aspx).


## Database high availability

The RavenDB database must be located in *shared storage* that is highly available and fault tolerant. Shared storage does not mean a  network share but shared cluster storage that allows low latency and exclusive access. Access to the data should always be 'local', although physically that data could be stored on a SAN. When this disk is mounted, RavenDB must be configured to use that location. See [Customize RavenDB Embedded Location](configure-ravendb-location.md) for more information on how to change the ServiceControl database location.


## ServiceControl detailed configuration

Once the failover cluster is created and ServiceControl is installed, configure ServiceControl to run in a clustered environment.

NOTE: The following steps must be applied to all ServiceControl installations on every node in the cluster.


### URL ACL(s)

ServiceControl exposes an HTTP API that is used by ServicePulse and ServiceInsight. URL ACL(s) must be [defined on each cluster node](/servicecontrol/setting-custom-hostname.md). The URL must be set to the `cluster name` and the ACL set to give permissions to the `Service Account` running ServiceControl.

NOTE: The default installation of ServiceControl locks down access to `localhost` only. Once the URL ACL is changed from `localhost` to the `cluster name` ServiceControl is accessible from the network.


### Configuration

ServiceControl configuration must be customized by changing the following settings:

* `DbPath` to define the path to the shared location where the database will be stored
* `Hostname` and `port` to reflect `cluster name` and `port`
*  `audit` and `error` queues to include the `cluster name`;

The following is a sample ServiceControl configuration file (ServiceControl.exe.config):

```xml
<configuration>
  <appSettings>
    <add key="ServiceControl/DbPath" 
         value="drive:\SomeDir\" />
    <add key="ServiceControl/Hostname" 
         value="clusterName" />
    <add key="ServiceControl/Port" 
         value="33333" />
    <add key="ServiceBus/AuditQueue"
         value="audit@clusterName" />
    <add key="ServiceBus/ErrorQueue" 
         value="error@clusterName" />
    <add key="ServiceBus/ErrorLogQueue" 
         value="error.log@clusterName" />
    <add key="ServiceBus/AuditLogQueue" 
         value="audit.log@clusterName" />
  </appSettings>
</configuration>
```

See [Customizing ServiceControl Configuration](/servicecontrol/creating-config-file.md) for more information.
