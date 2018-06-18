---
title: Upgrade Bridge 2 to Router 2
summary: Instructions on how to upgrade NServiceBus.Bridge Version 2 to NServiceBus.Router Version 2.
component: Bridge
reviewed: 2018-06-08
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

The NServiceBus.Bridge package has been deprecated and replaced by the more powerful NServiceBus.Router. The bridge-based and switch-and-port-based APIs available in the Bridge package have been replaced by a single router-interface-based API.

## Endpoint-side

The main change on the endpoint side is the use of `ConnectToRouter` instead of `ConnectToBridge`. As the switch model has been removed, the port configuration is no longer available.

snippet: bridge-to-router-connector

NOTE: When connecting to the router, use the name of router's interface, not the name of the router itself.


## Bridge to router

A simple bridge between two transports could be constructed using the `Bridge.Between<TLeft>(...).And<TRight>(...)` construct. This API is no longer available. The following code is an equivalent using the router APIs.

snippet: bridge-to-router-simple-router

A two-interface router is equivalent to a bridge but because the router can have any number of interfaces, it can no longer assume that messages should be forwarded to `Left` to `Right` and vice versa. The static routing protocol is used to configure the forwarding explicitly.

## Switch to router

A switch could be used to forward messages between more than two transports/brokers. The following code sets up a switch between sites A, B, and C.

snippet: bridge-to-router-switch

NOTE: When using a switch endpoints must be explicitly mapped to ports, either in the bridge connector or in the bridge itself.

The replacement router configuration is similar but uses interfaces instead of ports. The router interface mapping is more powerful than switch port mapping and allows for complex routing rules based on both the incoming interface and the destination. The result is the outgoing interface and, optionally, a next hop router (also known as gateway).

snippet: bridge-to-router-three-way-router
