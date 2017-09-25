---
title: WCF to messaging bridge
summary: Reliable WCF communication over messaging.
component: Wcf
reviewed: 2017-09-25
tags:
 - Hosting
related:
 - nservicebus/wcf
---

This sample demonstrates how the [WCF component](/nservicebus/wcf) can be leveraged to achieve reliable WCF services by bridging the WCF calls over messaging.


## Running the project

 1. Start the projects by hitting F5.
 1. The text `Press <enter> to send a message` and `Press <escape> to send a message which will time out` should be displayed in the Client's console window.
 1. Hit enter to send a message or escape to send a message which will time out.


### Verifying that the sample works correctly

 * When a message was sent with enter `Response 'Hello from handler'` will be displayed
 * When a message was sent with escape `Request failed due to: 'The request was cancelled after 00:00:05 because no response was received.'` will be displayed


## Code walk-through

The sample contains a self-hosted endpoint that leverages the WCF and the Callbacks package to enable reliable WCF services over messaging.

The [WCF](/nservicebus/wcf)) and the [Callback](/nservicebus/messaging/callbacks.md) component are enabled. The WCF component is configured to cancel requests after five seconds and a named pipe binding is used to expose the service as shown below:

snippet: enable-wcf

A channel factory is used to create a channel to communicate with the locally exposed WCF service

snippet: wcf-proxy

The request message is passed to the proxy as shown below

snippet: wcf-proxy-call

The proxy will asynchronously wait until the response or the cancellation is received. A response is sent from a regular handler hosted in the same endpoint

snippet: wcf-reply-handler