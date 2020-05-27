---
title: WCF to messaging bridge
summary: Reliable WCF communication over messaging.
component: Wcf
reviewed: 2019-06-25
related:
 - nservicebus/wcf
---

This sample shows how the [NServiceBus.Wcf package](/nservicebus/wcf) may be used to achieve reliable WCF services by bridging WCF calls over messaging.


## Running the sample

1. Open the solution in Visual Studio.
1. Press F5.
1. Follow the instructions in the client's console window.


### Verifying that the sample works correctly

 * When a message is sent which _does not_ timeout, `Response 'Hello from handler'` is displayed.
 * When a message is sent which _does_ timeout, `Request failed due to: 'The request was cancelled after 00:00:05 because no response was received.'` is displayed.


## Code walk-through

The sample contains a self-hosted endpoint which uses the [NServiceBus.Wcf](/nservicebus/wcf) and [NServiceBus.Callbacks](/nservicebus/messaging/callbacks.md) packages to achieve reliable WCF services over messaging.

The WCF package is configured to cancel requests after five seconds, and a named pipe binding is used to expose the service:

snippet: enable-wcf

A channel factory is used to create a channel to communicate with the locally exposed WCF service:

snippet: wcf-proxy

The request message is passed to the proxy:

snippet: wcf-proxy-call

The proxy waits, asynchronously, until the response or cancellation is received. A response is sent from a regular handler hosted in the same endpoint:

snippet: wcf-reply-handler