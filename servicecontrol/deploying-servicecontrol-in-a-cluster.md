---
title: Deploying ServiceControl in a cluster
summary: How to deploy ServiceControl in a Windows Cluster
tags:
- ServiceControl
- Cluster
- Windows Cluster
related:
- servicecontrol/troubleshooting
---

The following procedure is a high level guide on how to deploy ServiceControl onto a Microsoft fault tolerance Windows cluster.  This guide assumes that MSMQ is the underlying transport.


## Basic Setup

* Set up a Failover (active/passive) Windows cluster:
	* [Creating a Cluster in Windows Server 2008](http://blogs.msdn.com/b/clustering/archive/2008/01/18/7151154.aspx)
	* [Creating a Cluster in Windows Server 2012R2](https://technet.microsoft.com/en-us/library/dn505754.aspx)
* Install ServiceControl on each node adding it as a "Generic service" using the cluster manager. This means that ServiceControl will fail over automatically with the cluster.
* Set up a MSMQ Cluster Group. Cluster group is a group of resources that have a unique DNS name and can be addressed externally like a computer.
* Add the ServiceControl generic clustered service to the MSMQ cluster group:
	* Make it depend on MSMQ and MSMQ network name;
	* Check "use network name as computer name" in the service configuration;

Once set up ServiceControl queues will be available on the cluster. The server name will be the MSMQ network name, not to be confused with the cluster name.

More information on [Message Queuing in Server Clusters](https://technet.microsoft.com/en-us/library/cc753575.aspx).


## Database high availability

The RavenDB database needs to be located in shared storage, highly available and fault tolerant. See [Customize RavenDB Embedded Location](configure-ravendb-location.md) for more information on how to change the ServiceControl database location.


## ServiceControl detailed configuration

Once the Failover cluster is created and ServiceControl is installed, the next step is to configure ServiceControl to run in a clustered environment.

NOTE: The following steps needs to be applied to all the ServiceControl installation on every node in the cluster.


### URL ACL(s)

ServiceControl exposes an HTTP API that is used by ServicePulse and ServiceInsight. URL ACL(s) need to be [defined on each cluster node](/servicecontrol/setting-custom-hostname.md#updating-urlacl-settings). The URL needs to be the `cluster name` and the ACL is set to give permissions to the `Service Account` running ServiceControl.

NOTE: The default installation of ServiceControl locks down access to `localhost` only. Once the URL ACL is changed from `localhost` to the `cluster name` ServiceControl is accessible from the network.


### Configuration

ServiceControl configuration needs to be customized changing the following settings:

* `DbPath` to define the path to the shared location where the database will be stored;
* `Hostname` and `port` need to be updated to reflect `cluster name` and `port`
*  `audit` and `error` queues need to be updated to include the `cluster name`;

The following is a sample ServiceControl configuration file (ServiceControl.exe.config):

snippet:SCClusterAppSettings

See [Customizing ServiceControl Configuration](/servicecontrol/creating-config-file.md) for more information.
