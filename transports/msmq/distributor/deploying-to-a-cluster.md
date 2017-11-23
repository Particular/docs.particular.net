---
title: Deploying distributors to a Windows Failover Cluster
related:
- transports/msmq/troubleshooting
redirects:
 - nservicebus/scalability-and-ha/deploying-to-a-windows-failover-cluster
 - nservicebus/msmq/distributor/deploying-to-a-cluster
reviewed: 2017-11-15
---

Distributors are a single-point-of-failure if deployed to just a single machine. Make them highly available on a Windows Failover Cluster to avoid this.

Note: When using virtualization then usually the virtualization host itself can offer high availability as well as data redundancy. Windows Failover clustering might not be required, especially if the chosen virtualization layer supports live server migration.

The following procedure is a high-level guide on how to deploy distributors onto a Microsoft Windows Failover cluster.

NOTE: It is NOT required to use the distributor to achieve high-availability. A regular endpoint can also be deployed on a Windows Failover cluster.

 * This procedure assumes that the MSMQ transport uses distributed transactions (default). When using **SendsAtomicWithReceive** mode, then all steps related to the Distributed Transaction Coordinator (MSDTC) can be skipped.
 * This procedure assumes only a single cluster group, meaning MSMQ and MSDTC are installed once on a cluster node. This results in both MSMQ and MSDTC being active on the same node for improved performance, i.e. it allows to avoid a situation when MSMQ is active on node A and MSDTC is active on node B which would result in one additional network hop inside a distributor transaction.


## Windows Failover Cluster setup for distributors

 * Set up a Failover (active/passive) Windows Failover cluster:
  * [Creating a Cluster in Windows Server 2008](https://blogs.msdn.microsoft.com/clustering/2008/01/18/creating-a-cluster-in-windows-server-2008/).
  * [Creating a Cluster in Windows Server 2012R2 and Windows Server 2016](https://technet.microsoft.com/en-us/library/dn505754.aspx).
 * Setup a new cluster group. A cluster Group is a group of resources that have a unique NETBIOS network name and can be addressed externally which is required for MSMQ.
 * Install clustered Message Queuing (MSMQ) and Distributed Transaction Coordinator (MSDTC) into the new cluster group.
  * Install the Message Queuing Services on each node.
  * Use the Failover Cluster Manager to [add Message Queuing as a clustered role](https://blogs.msdn.microsoft.com/asiatech/2016/01/14/build-clustered-msmq-role-on-a-windows-server-2012-r2-failover-cluster/).
  * Use the Failover Cluster Manager to add MSDTC as a clustered role.
 * Install distributors as a clustered Generic Service.
  * [Install the distributor as a standard windows service](/nservicebus/hosting/#self-hosting-windows-service-hosting) on each node.
  * Add it as a clustered "Generic service" using the Failover Cluster Manager.
  * Add dependencies to the following cluster group resources: network name, MSMQ, and MSDTC.
  * Check "use network name as computer name".
  * Repeat for each distributor.

Once set up, the clustered MSMQ queues and the distributors will be available on the network. Use the network name configured on the cluster group to reach them. Make sure to direct any send operations or messages subscriptions for endpoints using the distributor to the new network name.


More information on [Message Queuing in Server Clusters](https://technet.microsoft.com/en-us/library/cc753575.aspx).
