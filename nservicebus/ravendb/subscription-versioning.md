---
title: Subscription versioning
component: raven
tags:
 - RavenDB
 - Persistence
 - Subscription
reviewed: 2016-11-28
---

The following section outlines the way subscriptions are handled when persisted using RavenDB.

By design the version of the event subscribers are subscribed to should be ignored by publishers, allowing a version mismatch between the assembly at the subscriber and the assembly at the publisher. The main reason for this is to simplify endpoints evolution without locking subscribers and publishers eveolution due to message assembly versions.

RavenDB subscription persistance, unfortunately due to legacy reasons, does not respect this rule and requires the major version (taken from assembly version) to match the version of the contract a subscriber has subscribed to for a publish operation to include the given subscriber. This means that if the publisher changes the major version without the subscriber being updated as well the subscriber will not get the published event.

## Technical details

-- describe why this can't be fixed in a patch release?

## Workarounds

-- do not vwersion message assemblies
-- use the AssemblyFileVersion attribute instead of AssemblyVersion
-- use the AssemblyInformationalVersion attribute instead of AssemblyVersion
