<!--
title: "Migrating To NServiceBus 3.0 – Timeouts"
tags: ""
summary: "<p>This article describes your options when migrating your V2.6 timeouts to the new V3.0 format.</p>
<p>If you are not familiar with the NServiceBus timeouts, in brief, NServiceBus supports durable timeouts that survive process restarts. To do that, you need to store the timeouts on disk.</p>
"
-->

This article describes your options when migrating your V2.6 timeouts to the new V3.0 format.

If you are not familiar with the NServiceBus timeouts, in brief, NServiceBus supports durable timeouts that survive process restarts. To do that, you need to store the timeouts on disk.

In V2.6 the default storage was an MSMQ queue, but V3.0 uses RavenDB, so you might need to migrate. It may not be necessary because the actual timeout messages sent over the wire are compatible between V2.6 and V3.0.X.

**NOTE** : The reason to use NServiceBus 3.0.X for the timeout to work is that a bug in V3.0.0 made it incompatible. The bug is fixed in 3.0.X. This means you can run the V2.6 and V3.0.X TimeoutManagers (TM) in parallel until there are no more V2.6 timeouts left, and then decommission the V2.6 TM.

To skip migration and run the TimeoutManagers side by side:

1.  Upgrade your endpoint to V3.0.X. Download it from the
    [downloads](/downloads) page.
2.  Configure the endpoint to use the built-in TM in V3.0. New timeouts
    will be sent to this TM from your endpoint.
3.  Keep the V2.6 TM running. Existing timeouts that expire will be sent
    to your new V3.0.X endpoint. (Make sure that you keep the name of
    the input queue identical.)
4.  Decommission your V2.6 TM when all timeouts expire. (The storage
    queue will be empty when this happens.) The default name of the
    storage queue is “Timeout.Storage” but check your configuration to
    be sure. **NOTE** : This is NOT the same queue as the input queue
    that you would have configured in your endpoint mappings.

Why to migrate?
---------------

The fact that timeouts are durable means that they could—and usually are—set to a time very far off in the future. For example, if you have insurance with long cycles you can have your renewal saga set to wake up in X years. In this situation you don’t want to run both timeout managers in parallel for that long a time. This is when you would consider doing a migration instead.

To do this we provide a tool in the ZIP download
(/Tools/Migration/TimeoutMigrator.exe) or in the NServiceBus.Tools NuGet package. This tool extracts your V2.6 timeouts and sends them to be managed by the new V3.0 TM.

For those of you who require zero downtime deployments, V2.6.0.1504 doesn’t support hot migrations. This means that to migrate your timeouts with your system still running, you need to upgrade to NServiceBus V2.6.0.1511.

With that out of the way, use the tool to migrate:

1.  Upgrade your endpoint to V3.0.X.
2.  Create the [dedicated input queue for the V3.0
    TM](convention-over-configuration) by running the
    [installers](nservicebus-installers.md) .
3.  If you haven’t upgraded to
    [V](http://particular.cloudapp.net/downloads)
    [2.6.0.1511](http://particular.cloudapp.net/downloads) , shut down
    the V2.6 TM.
4.  Run the TimeoutMigrator.exe. To migrate only timeouts older than a
    specific time, use the -migrateOlderThan {minutes} switch. This
    extracts the timeouts and sends them to the new V3.0 TM. The tool
    asks you for the source and destination queues if not specified on
    the command line.

Command line switches:

-   storageQueue {name of the V2.6 storage queue}
-   destination {name of the V3.0 TM input queue}

Typical settings:

-   storageQueue Timeout.Storage
-   destination {endpointName}.Timeouts

NOTES
-----

-   Make sure to use NServiceBus V3.0.1 or higher
-   Update to NServiceBus V2.6.0.1511 if you need to do a zero downtime
    migration
-   Make sure you have the correct settings by testing the process in
    your testing environment


