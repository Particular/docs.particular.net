---
title: Repair malformed messages using pipeline behavior
summary: A sample showing how repair malformed messages by implementing a pipeline behavior
component: Core
reviewed: 2020-10-17
---

This sample shows how to repair a malformed message by implementing a pipeline behavior. The sample uses the [Learning Transport](/transports/learning/) and a portable version of the Particular Service Platform tools. Installing ServiceControl is **not** required.

include: platformlauncher-windows-required

downloadbutton


## Running the project

Running the project will result in 4 console windows. Wait a moment until the ServicePulse window opens in the browser.

 1. Press <kbd>Enter</kbd> in the Sender console window.
 2. Observe error log output in the Receiver console window: the endpoint cannot process the message because the `Id` field is malformed - it contains lowercase characters.
 3. Switch to ServicePulse. It should show one failed message. Hit the retry button and wait until the message is retried.
 4. Observe error log output in the Receiver console window. The endpoint still can't process the message.
 5. Stop the Receiver project.
 6. Update the Receiver configuration code to register the `FixMessageIdBehavior` behavior in the pipeline by uncommenting the code in the `RegisterFixBehavior` region.
 7. Start the updated Receiver endpoint.
 8. Go back to `Failed Messages` tab, select the failed message and hit the retry button again.
 9. Switch to the Receiver console window and observe the successful processing notification.


## Code walk-through 


### Sender

Sends messages of `SimpleMessage` type and emulates a bug by sending malformed `Id` field values.


### Receiver

Retries are disabled in the sample for simplicity; messages are immediately moved to the error queue after a processing failure:

snippet: DisableRetries

This endpoint processes messages of type `SimpleMessage` and expects the `Id` field to not contain any lowercase characters. Messages with lowercase characters are rejected (and sent to the error queue).

snippet: ReceiverHandler

To fix failing messages the endpoint defines a pipeline behavior to convert lower-case message identifiers to upper-case.

snippet: RegisterFixBehavior

where `FixMessageIdBehavior` is an incoming pipeline behavior

snippet: FixMessageIdBehavior
