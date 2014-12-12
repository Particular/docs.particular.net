---
title: Deploying ServiceControl in a cluster
summary: How to deploy ServiceControl in a Windows Cluster
originalUrl: 
tags:
- ServiceControl
- Cluster
- Windows Cluster

---
## Deploying ServiceControl in a cluster

###Basic Setup
Set up an active/passive Windows cluster. 

Install ServiceControl on each node and then added as a "Generic service" in cluster manager. 
This means ServiceControl fails over automatically with the cluster. 

NOTE: Because ServiceControl is running in the context of the cluster, you need to have a clustered MSMQ resource if you want a resilient ServiceControl instance because you have to have a single queue address. 

A single queue address needs to be a clustered queue (transactional MSMQ does not work behind a load balancer).

Put the ServiceControl service in the MSMQ group, make it depend on MSMQ and MSMQ network name, and check “use network name as computer name” in the service configuration. 

After you do all that you can have “local” queues on the cluster, using the MSMQ network name as the server name (note that this is NOT the cluster name).

NOTE: Having MSMQ use the local queues on the node (which means data loss after failover) or use of the clustered queues as remote queues, will result in a serious performance hit and reduced reliability.

###Things to consider...
#### RavenDB
The RavenDB database need to be located in shared storage.

#### ServiceControl ACLs
You need to set the URL ACLs on each cluster node [as advised here:](/servicecontrol/troubleshooting.md)
The URL needs to be the cluster name and the ACL obviously set to whatever
service account is running service control

You need to create a ServiceControl configuration file and it needs to look
something like this:

<!-- import SCClusterAppSettings -->

NOTE: for some reason, some of those AppSettings are prefixed with
"ServiceBus" (not ServiceControl). Those aren't typos.

#### ServiceControl's Logs
If you look at ServiceControl's [Troubleshooting URL](/servicecontrol/troubleshooting.md) it tells you
where the ServiceControl log file is in case you have any problems starting
the service.
