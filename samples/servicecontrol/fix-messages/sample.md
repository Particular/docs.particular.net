---
title: Repair malformed messages using ServiceControl retry redirects 
summary: A sample showing how repair malformed messages by redirecting them to a temporary endpoint
component: Core
reviewed: 2020-10-24
---

This sample shows how to repair a malformed message by redirecting them to a temporary endpoint. The sample uses the [Learning Transport](/transports/learning/) and a portable version of the Particular Service Platform tools. Installing ServiceControl is **not** required.

include: platformlauncher-windows-required

downloadbutton


## Running the project

Running the project will result in 4 console windows. Wait a moment until the ServicePulse window opens in the browser.

 1. Press <kbd>Enter</kbd> in the Sender console window
 2. Observe error log output in the Receiver console window: the endpoint cannot process the message because the `Id` field is malformed -- it contains lowercase characters.
 3. Switch to ServicePulse. It should show one failed message. Hit the retry button and wait until the message is retried.
 4. Observe error log output in the Receiver console window. The endpoint still can't process the message.
 5. Switch to ServicePulse again. Go to `Configuration` tab and select `Retry redirects`.
 6. Create a redirect from `FixMalformedMessages.Receiver` to `FixMalformedMessages.MessageRepairingEndpoint`.
 7. Go back to `Failed Messages` tab, select the failed message and hit the retry button again.
 8. Switch to the MessageRepairingEndpoint console window and observe the log entries saying the message has been repaired and forwarded.
 9. Switch to the Receiver console window and observer the successful processing notification.


## Code walk-through 


### Receiver

Retries are disabled in the sample for simplicity; messages are immediately moved to the error queue after a processing failure:

snippet: DisableRetries

This endpoint processes messages of type `SimpleMessage` and expects the `Id` field to not contain any lowercase characters. Messages with lowercase characters are rejected (and sent to the error queue).

snippet: ReceiverHandler


### Sender

Sends messages of `SimpleMessage` type and emulates a bug by sending malformed `Id` field values.


## MessageRepairingEndpoint

This is a disposable one-time endpoint to be used to repair malformed messages sent by the Sender endpoint. In real life scenario it can be removed as soon as the bug in Sender is fixed. It contains a handler for `SimpleMessage` that repairs the messages and forwards them to the Receiver.

snippet: RepairAndForward
