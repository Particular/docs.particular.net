---
title: "Migrating To NServiceBus 3.0 – Time-outs"
summary: NServiceBus Version 3.0 supports durable time-outs that survive process restarts. Store the time-outs on disk.
tags: []
redirects:
 - nservicebus/migrating-to-nservicebus-3.0-timeouts
---

This article describes your options when migrating your Version 2.6 time-outs to the new Version 3.0 format.

If you are not familiar with the NServiceBus time-outs, in brief, NServiceBus supports durable time-outs that survive process restarts. To do that, you need to store the time-outs on disk.

In Version 2.6 the default storage was an MSMQ queue, but Version 3.0 uses RavenDB, so you might need to migrate. It may not be necessary because the actual time-out messages sent over the wire are compatible between Version 2.6 and Version 3.0.X.

NOTE: The reason to use NServiceBus 3.0.X for the time-out to work is that a bug in Version 3.0.0 made it incompatible. The bug is fixed in 3.0.X. This means you can run the Version 2.6 and Version 3.0.X TimeoutManagers (TM) in parallel until there are no more Version 2.6 timeouts left, and then decommission the Version 2.6 TM.

To skip migration and run the TimeoutManagers side by side:

 1. Upgrade your endpoint to Version 3.0.X.
 1. Configure the endpoint to use the built-in TM in Version 3.0. New time-outs will be sent to this TM from your endpoint.
 1. Keep the Version 2.6 TM running. Existing time-outs that expire will be sent to your new Version 3.0.X endpoint. (Make sure that you keep the name of the input queue identical.)
 1. Decommission your Version 2.6 TM when all time-outs expire. (The storage queue will be empty when this happens.) The default name of the storage queue is "Timeout.Storage" but check your configuration to be sure.

NOTE: This is NOT the same queue as the input queue that you would have configured in your endpoint mappings.

## Why to migrate?

The fact that time-outs are durable means that they could and usually are set to a time very far off in the future. For example, if you have insurance with long cycles you can have your renewal saga set to wake up in X years. In this situation you don't want to run both time-out managers in parallel for that long a time. This is when you would consider doing a migration instead.

To do this we provide a tool in the ZIP download (`/Tools/Migration/TimeoutMigrator.exe`) or in the NServiceBus.Tools NuGet package. This tool extracts your V2.6 time-outs and sends them to be managed by the new Version 3.0 TM.

For those of you who require zero downtime deployments, Version 2.6.0.1504 doesn't support hot migrations. This means that to migrate your time-outs with your system still running, you need to upgrade to NServiceBus Version 2.6.0.1511.

With that out of the way, use the tool to migrate:

 1. Upgrade your endpoint to Version 3.0.X.
 1. Create the dedicated input queue for the Version 3.0 TM by running the [installers](/nservicebus/operations/installers.md) .
 1. If you haven't upgraded to Version 2.6.0.1511, shut down the Version 2.6 TM.
 1. Run the TimeoutMigrator.exe. To migrate only time-outs older than a specific time, use the -migrateOlderThan {minutes} switch. This extracts the time-outs and sends them to the new Version 3.0 TM. The tool asks you for the source and destination queues if not specified on the command line.

Command line switches:

 * storageQueue {name of the Version 2.6 storage queue}
 * destination {name of the Version 3.0 TM input queue}

Typical settings:

 * storageQueue Timeout.Storage
 * destination {endpointName}.Timeouts


## NOTES

 * Make sure to use NServiceBus Version 3.0.1 or higher
 * Update to NServiceBus Version 2.6.0.1511 if you need to do a zero downtime migration
 * Make sure you have the correct settings by testing the process in your testing environment