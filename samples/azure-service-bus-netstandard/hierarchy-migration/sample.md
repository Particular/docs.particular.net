---
title: Azure Service Bus Hierarchy Migration Sample
summary: Demonstrates how to life migrate an endpoint from one topic to another
reviewed: 2022-12-06
component: ASBS
related:
- transports/azure-service-bus
---

NOTE: This sample is available to aid and gauge interest in this migration approach. Depending on the feedback, the functionality of migrating an endpoint could be built into the [operational scripting tool](/transports/azure-service-bus/operational-scripting.md) of the Azure Service Bus transport. Comment on [the issue](https://github.com/Particular/NServiceBus.Transport.AzureServiceBus/issues/718) or reach out using the [support options available](https://particular.net/support).

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample demonstrates how an endpoint can be migrated live and without any downtime from one topic to another:

* `Endpoint1` publishes `Event1` messages and subscribes to `Event2` messages.
* `Endpoint2` subscribes to `Event1` messages and publishes `Event2` messages.
* `Migration` does a step by step live migration of `Endpoint2` while the endpoints are running and exchanging messages with each other.

### Sample output

The migration program creates self-explanatory output during the migration process.

```text
----- Endpoints should be stopped ------
⚠ Make sure 'Endpoint1' and 'Endpoint2' are not running

------ Infrastructure cleanup ------
Cleanup infrastructure from previous runs of this sample? [y/n] (y): y
------ Deleted infrastructure ------
┌────────────────────────┐
│     Deleted Topics     │
├────────────────────────┤
│  bundle-to-publish-to  │
│ bundle-to-subscribe-to │
└────────────────────────┘
┌─────────────────────────────────────────────────────┐
│                   Deleted Queues                    │
├─────────────────────────────────────────────────────┤
│      samples.asbs.hierarchymigration.endpoint1      │
│      samples.asbs.hierarchymigration.endpoint2      │
│ samples.asbs.hierarchymigration.endpoint2.migration │
└─────────────────────────────────────────────────────┘

----- Endpoints should be running ------
⚠ Start 'Endpoint1' and 'Endpoint2' and wait a bit until some messages are published.

------ Topology before migration -----
Topics
└── bundle-to-publish-to
    ├── Samples.ASBS.HierarchyMigration.Endpoint1
    │   ├── $default: 1=0
    │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
    │   └── samples.asbs.hierarchymigration.endpoint1
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        └── samples.asbs.hierarchymigration.endpoint2
Queues
├── samples.asbs.hierarchymigration.endpoint1
└── samples.asbs.hierarchymigration.endpoint2
:: Press any key to setup the sample topology

----- The 'bundle-to-subscribe-to' topic has been setup ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   └── Samples.ASBS.HierarchyMigration.Endpoint2
│       ├── $default: 1=0
│       ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
│       └── samples.asbs.hierarchymigration.endpoint2
└── bundle-to-subscribe-to
Queues
├── samples.asbs.hierarchymigration.endpoint1
└── samples.asbs.hierarchymigration.endpoint2
----- The forward rule on topic 'bundle-to-publish-to' has been setup ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   ├── Samples.ASBS.HierarchyMigration.Endpoint2
│   │   ├── $default: 1=0
│   │   ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
│   │   └── samples.asbs.hierarchymigration.endpoint2
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
Queues
├── samples.asbs.hierarchymigration.endpoint1
└── samples.asbs.hierarchymigration.endpoint2
----- The migration queue 'samples.asbs.hierarchymigration.endpoint2.migration' has been setup -----
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   ├── Samples.ASBS.HierarchyMigration.Endpoint2
│   │   ├── $default: 1=0
│   │   ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
│   │   └── samples.asbs.hierarchymigration.endpoint2
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
Queues
├── samples.asbs.hierarchymigration.endpoint1
├── samples.asbs.hierarchymigration.endpoint2
└── samples.asbs.hierarchymigration.endpoint2.migration
----- The subscription 'Samples.ASBS.HierarchyMigration.Endpoint2' on 'topic bundle-to-subscribe-to' has been setup ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   ├── Samples.ASBS.HierarchyMigration.Endpoint2
│   │   ├── $default: 1=0
│   │   ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
│   │   └── samples.asbs.hierarchymigration.endpoint2
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        └── samples.asbs.hierarchymigration.endpoint2.migration
Queues
├── samples.asbs.hierarchymigration.endpoint1
├── samples.asbs.hierarchymigration.endpoint2
└── samples.asbs.hierarchymigration.endpoint2.migration
----- The subscription 'Samples.ASBS.HierarchyMigration.Endpoint2' on topic 'bundle-to-subscribe-to' with the necessary rules ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   ├── Samples.ASBS.HierarchyMigration.Endpoint2
│   │   ├── $default: 1=0
│   │   ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
│   │   └── samples.asbs.hierarchymigration.endpoint2
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        └── samples.asbs.hierarchymigration.endpoint2.migration
Queues
├── samples.asbs.hierarchymigration.endpoint1
├── samples.asbs.hierarchymigration.endpoint2
└── samples.asbs.hierarchymigration.endpoint2.migration
Press any key to continue with the migration

----- The subscription 'Samples.ASBS.HierarchyMigration.Endpoint2' on topic 'bundle-to-publish-to' has been deleted ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        └── samples.asbs.hierarchymigration.endpoint2.migration
Queues
├── samples.asbs.hierarchymigration.endpoint1
├── samples.asbs.hierarchymigration.endpoint2
└── samples.asbs.hierarchymigration.endpoint2.migration
----- Checking migration queue status ------
⚠ There are currently 3 messages in the migration queue.

----- The queue 'samples.asbs.hierarchymigration.endpoint2.migration' forwarding to 'samples.asbs.hierarchymigration.endpoint2' has been setup -----
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        └── samples.asbs.hierarchymigration.endpoint2.migration
Queues
├── samples.asbs.hierarchymigration.endpoint1
├── samples.asbs.hierarchymigration.endpoint2
└── samples.asbs.hierarchymigration.endpoint2.migration
    └── samples.asbs.hierarchymigration.endpoint2
----- Checking forwarding progress in the migration queue ------
⌛ Everything forwarded. Moving on
------ The subscription 'Samples.ASBS.HierarchyMigration.Endpoint2' on topic 'bundle-to-subscribe-to' has been changed to forward to… ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        └── samples.asbs.hierarchymigration.endpoint2
Queues
├── samples.asbs.hierarchymigration.endpoint1
├── samples.asbs.hierarchymigration.endpoint2
└── samples.asbs.hierarchymigration.endpoint2.migration
    └── samples.asbs.hierarchymigration.endpoint2
Press any key to continue with the migration

------ The queue 'samples.asbs.hierarchymigration.endpoint2.migration' was removed ------
Topics
├── bundle-to-publish-to
│   ├── Samples.ASBS.HierarchyMigration.Endpoint1
│   │   ├── $default: 1=0
│   │   ├── Event2: [NServiceBus.EnclosedMessageTypes] LIKE '%Event2%'
│   │   └── samples.asbs.hierarchymigration.endpoint1
│   └── forwardTo-bundle-to-subscribe-to
│       ├── $default: 1=1
│       └── bundle-to-subscribe-to
└── bundle-to-subscribe-to
    └── Samples.ASBS.HierarchyMigration.Endpoint2
        ├── $default: 1=0
        ├── Event1: [NServiceBus.EnclosedMessageTypes] LIKE '%Event1%'
        └── samples.asbs.hierarchymigration.endpoint2
Queues
├── samples.asbs.hierarchymigration.endpoint1
└── samples.asbs.hierarchymigration.endpoint2
🏅 Migration done.
Stop 'Endpoint2' and change:
`transport.Topology = TopicTopology.Single("bundle-to-publish-to");`

to

`transport.Topology = TopicTopology.Hierarchy("bundle-to-publish-to", "bundle-to-subscribe-to");`

and then start 'Endpoint2'.
```

## Viewing in-flight messages

For every supported migration step, the migration console will wait for any key to continue, which allows for inspecting the queues, topics, and subscriptions on the Azure Portal or a third-party tool during the migration process.

The following queues for the two endpoints can be viewed in the Azure Portal or a third-party tool:

* `samples.asbs.hierarchymigration.endpoint1`
* `samples.asbs.hierarchymigration.endpoint2`
* `samples.asbs.hierarchymigration.endpoint2.migration`

The following topics can be inspected:

* `bundle-to-publish-to`
* `bundle-to-subscribe-to`
