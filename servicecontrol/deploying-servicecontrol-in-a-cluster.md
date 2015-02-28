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
- Set up an active/passive Windows cluster. 

- Install ServiceControl on each node and then added as a "Generic service" in cluster manager. 
This means ServiceControl fails over automatically with the cluster. 

NOTE: Because ServiceControl is running in the context of the cluster, you need to have a clustered MSMQ resource if you want a resilient ServiceControl instance because you have to have a single queue address. 

NOTE: A single queue address needs to be a clustered queue (transactional MSMQ does not work behind a load balancer).

- Put the ServiceControl service in the MSMQ group, make it depend on MSMQ and MSMQ network name, and check “use network name as computer name” in the service configuration. 

- Once you complete the steps above you can have “local” queues on the cluster, using the MSMQ network name as the server name (note that this is NOT the cluster name).

###Things to consider...
#### RavenDB
The RavenDB database need to be located in shared storage. See [Customize RavenDB Embedded Location](configure-ravendb-location.md)

#### ServiceControl ACLs
You need to set the URL ACLs on each cluster node [as advised here:](/servicecontrol/troubleshooting.md)
The URL needs to be the cluster name and the ACL is set to whatever service account is running service control.

NOTE: Default install of SC locks down access to localhost only. Once the URLACL is changed from localhost to the cluster name as instructed here it is now accessible from the network which I realize is partially the point but I do think it worth prompting the user to consider securing this access (Firewall or VPN perhaps).

You need to create a ServiceControl configuration file (ServiceControl.exe.Config), setting the following:
- `DbPath` to the shared Database path
- Setting the `Hostname` and `port` to the cluster name and port
-  `audit` and `error` queues on the cluster

<!-- import SCClusterAppSettings -->

#### ServiceControl's Logs
If you look at ServiceControl's [Troubleshooting URL](/servicecontrol/troubleshooting.md) it tells you
where the ServiceControl log file is in case you have any problems starting
the service.
