---
title: Deploying in a Windows Failover Cluster
summary: Steps necessary to install NServiceBus Distributor endpoints in a windows failover cluster.
tags:
- Failover
- Scalability
- Distributor
redirects:
 - nservicebus/deploying-nservicebus-in-a-windows-failover-cluster
---

NServiceBus is designed for scalability and reliability, but to take advantage of these features, it is necessary to deploy it in a Windows Failover Cluster. Unfortunately, information on how to do this effectively is, as yet, incomplete and scattered. This article describes the process for deploying NServiceBus in a failover cluster. This article does not cover the generic setup of a failover cluster. There are other, better resources for that, such as [Creating a Cluster in Windows Server 2008](https://blogs.msdn.microsoft.com/clustering/2008/01/18/creating-a-cluster-in-windows-server-2008/) or [Server 2012](https://technet.microsoft.com/en-us/library/dn505754.aspx). The focus here is the setup related to NServiceBus.


## Planning the infrastructure

A simple setup for scalability and reliability includes at least two servers in a failover cluster. The failover cluster servers run a distributor process with a timeout manager for each logical message queue.

In addition there must exist one or more additional servers called worker nodes. These contain endpoints containing message handlers and they are the servers that can be scaled out. The endpoints on worker nodes request work from the clustered distributors, do the work, and then ask for more.


## Setting up the clustered service

While technically it shouldn't matter from which clustered server is set up, generally it is more reliable to set up everything from whichever server currently holds the Quorum disk. Find the server that has it (it moves around when the server holding it restarts), and open up Failover Cluster Management under Administrative Tools.

Set up a clustered DTC access point:

 1. Right click Services and Applications and select Configure a Service or Application.
 1. Go past the intro screen, select Distributed Transaction Coordinator (DTC) and click Next.
 1. Follow the wizard, assigning a DNS name for the clustered DTC (e.g., ClusterNameDTC), IP address, and storage.
 1. When finished service the DTC icon will exist.

Configure DTC for NServiceBus:

 1. On each server, in `Administrative Tools - Component Services`, expand `Component Services - Computers - My Computer - Distributed Transaction Coordinator`.
 1. For the Local DTC, if the clustered DTC is on the current node, note the Clustered DTCs directory with the clustered DTC name inside it.
 1. For both instances (so three times counting each node and the clustered instance), right-click, select Properties, and switch to the Security tab.
 1. At the very least, check "Network DTC Access" and "Allow Outbound."
 1. Optionally, check "Allow Remote Clients" and "Allow Inbound."

Set up a MSMQ Cluster Group. Cluster group is a group of resources that have a unique DNS name and can be addressed externally like a computer.

 * In order to create a cluster group up in Windows 2003, create a "cluster group" in the admin
 * In Windows 2008, create a "cluster service or application"
 * In Windows 2012, create a "cluster role"

For more information, see https://technet.microsoft.com/en-us/library/cc753575.aspx

For NServiceBus endpoint destination, queues are addressed by the MSMQ cluster group's name, clustered resources be added will later. In non-cluster terms, typically the machine name is added to the address of the queue, i.e. `queue@MachineName`. In cluster terms it is addressed by queue@MSMQ Network name.

WARNING: These queue(s) must be manually created as they are not created by NServiceBus installation process

In Failover Cluster Management, from the server with Quorum:

 1. Right click Services and Applications and select Configure a Service or Application.
 1. Skip the intro, and select Message Queuing.
 1. Finish the wizard, configuring the MSMQ Network Name, IP address, and storage.

This will result in a clustered MSMQ instance. Click the instance under Services and Applications to see the summary, which contains the Server Name, Storage, and MSMQ instance. Right click the MSMQ instance, select Properties, and the Dependencies tab. Make sure that it contains dependencies for the MSMQ Network Name AND IP Address AND Storage.

View the MSMQ MMC snap-in by right clicking the MSMQ cluster in the left pane and selecting Manage MSMQ, which opens the Computer Management tool geared toward the clustered instance.

Go to MSMQ by expanding Services and Applications - Message Queuing.

Keep in mind that this only to work if viewing Failover Cluster Management from the server where the MSMQ Network Name currently resides. If on Server A and an attempt is made to manage MSMQ on a MSMQ Network residing on Server B, Message Queuing will not show up in the Computer Management window.

Try swapping the MSMQ Network Name back and forth between nodes a few times. It's best to make sure that everything is working properly now before continuing.


## More about MSMQ Network Name

The "cluster name" is a Network Name created for the cluster as part of the core Cluster Group - a group created by default for each cluster. The core cluster group is different than the MSMQ cluster group and it has a Different network name. One of the most common confusions while using MSMQ on a cluster is using the Cluster Name in the client instead of the MSMQ Network Name.

![](failover-cluster-management.png "(Picture taken from https://blogs.technet.microsoft.com/askcore/2009/02/17/how-to-configure-multiple-instances-of-distributed-transaction-coordinator-dtc-on-a-windows-server-failover-cluster-2008/")

In this picture:

 * The Cluster Name is W2K8-FC (note that in the upper left corner of the picture)
 * The MSMQ cluster group's name is MSMQ-1
 * The MSMQ network name is named MSMQ-1 (this is the first resource in the group - under "Name: MSMQ-1"). The MSMQ Network Name (DNS Name) is not shown in this picture - to see it right click "Name: MSMQ-1" and select "Properties". Notice something like (Note - this was taken from another resource):

![](cluster-properties.png "Cluster Properties")

NOTE: Under "DNS Name" find the MSMQ DNS Name, which may or may not be "MSMQ-1".


## Installing the clustered distributor services

Before the `NServiceBus.Host.exe` processes can be clustered, they need to be installed services on all clustered nodes.

Copy the Distributor binary as many times as there are have logical queues, and then configure each one as described in the [NServiceBus Distributor](distributor/) page. To keep everything straight, the queues are named according to the following convention:

 * Distributor Data Bus: EndpointName
 * Distributor Control Bus: EndpointName.Distributor.Control
 * Distributor Storage Queue: EndpointName.Distributor.Storage

**A review of how the distributor works:** Other endpoints send messages to the queue specified by the Distributor Data Bus, where they accumulate if no worker is running. When a worker comes online, it sends a ReadyMessage to the queue specified by the Distributor Control Bus. If there is work to be done, the distributor sends an item from the Data Bus to the endpoint's local input queue, otherwise, it files it in the Distributor Storage Queue so that when work does come in, the distributor knows who is ready to process it.

Using this naming convention, all the queues of an endpoint end up grouped together.

Install each distributor from the command line:

```cs
NServiceBus.Host.exe
/install
/serviceName:EndpointName
/displayName:Distributor.EndpointName
/description:Distributor.EndpointName
/userName:DOMAIN\usa
/password:thepassword
NServiceBus.Production NServiceBus.MSMQDistributor
```

It's easier to set the service name, display name, and description to be the same. It helps when trying to start and stop things from a NET START/STOP command and when viewing them in the multiple graphical tools. Starting each one with Distributor puts them all together alphabetically in the Services MMC snap-in.

Don't forget the `NServiceBus.Production` at the end, which sets the profile for the NServiceBus generic host, as described in the [Generic Host page](/nservicebus/hosting/nservicebus-host/) and the `NServiceBus.MSMQDistributor` which sets up the host in distributor mode.

Do not try starting the services as they will run in the scope of the local server node, and will attempt to create their queues there.

Now, add each distributor to the cluster:

 1. Right-click the MSMQ cluster group, and select Add a Resource - \#4 Generic Service.
 1. Select the distributor service from the list. The services are listed in random order. Typing "Distributor" will help locate the desired service.
 1. Finish the wizard. The service should be added to the cluster group, but not activated. Don't activate it yet!
 1. Right click the distributor resource and select Properties.
 1. Now this is where it gets weird. Eventually check "Use Network Name for computer name" and add a dependency, but do not do both at the same time! If done at the same time it will complain that it can't figure out what the network name is supposed to be because it can't find it in the dependency chain but it hasn't been saved yet. To get around it, switch to the Dependencies tab and add a dependency for the MSMQ instance. From there, it finds everything else by looking up the tree. Click Apply to save the dependency.
 1. Switch back to the General tab and check the "Use Network Name for computer name" checkbox. This tells the application that `Environment.MachineName` should return the cluster name, not the cluster node's computer name. Click Apply.
 1. Repeat for the other distributors.

Again, try swapping the cluster back and forth, to ensure it can move freely between the cluster nodes.


## Setting up the workers

include: distributor-inV6

Ensure that the worker servers have [unique QMIds](distributor/#worker-qmid-needs-to-be-unique).

Set up the worker processes on all worker servers (not the cluster nodes!) as windows services, but instead of using `NServiceBus.MSMQDistributor`, use `NServiceBus.MSMQWorker` profile instead.

Configure the workers' `MasterNodeConfig` section to point to the machine running the distributor as described on the Distributor Page under [Routing with the Distributor](distributor).

With the distributors running in the cluster and the worker processes coming online, note the Storage queues for each process start to fill up. The more worker threads configured, the more messages will appear in each Storage queue.

While in development, the endpoint configurations probably don't have any `@` symbols in them, in production change to point to the Data Bus queue on the cluster, i.e., for application MyApp and logical queue MyQueue, the worker config looks like this:


```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <!-- Other sections go here -->
    <section name="MasterNodeConfig" type="NServiceBus.Config.MasterNodeConfig, NServiceBus.Core" />
    <section name="UnicastBusConfig" type="NServiceBus.Config.UnicastBusConfig, NServiceBus.Core"/>
  </configSections>
  <!-- Other config options go here -->
  <MasterNodeConfig Node="MachineWhereDistributorRuns"/>

  <UnicastBusConfig">
    <MessageEndpointMappings>
      <!-- regular entries -->
    </MessageEndpointMappings>
  </UnicastBusConfig>

</configuration>
```


## Conclusion

This article shows how to set up a Windows Failover Cluster and one or more worker node servers to run a scalable, maintainable, and reliable NServiceBus application infrastructure.

 * Scaling up can be achieved by adjusting the number of threads on each worker process.
 * Scaling out can be achieved by starting up another server to run another worker process connected to the clustered distributor.
 * For maintenance, the worker processes on either worker server can be stopped for server maintenance or application updates, while worker processes on the other server continue to process messages. All clustered resources can be failed over to one node without disrupting the system, allowing message processing to continue while the other clustered node is available for updates.
 * Reliability is achieved by never requiring that any one component be completely shut down.
