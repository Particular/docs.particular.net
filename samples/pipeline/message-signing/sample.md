---
title: Message signing using the pipeline
summary: Shows how to implement message signing and verification using NServiceBus pipeline behaviors.
reviewed: 2019-10-03
component: Core
related:
 - samples/pipeline/handler-timer
 - samples/pipeline/unit-of-work
---

This sample demonstrates how to implement message signing for messages in an NServiceBus system, in order to validate that a sender of a message is valid. The sample implements the message signing and verification as two separate [NServiceBus pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md): one to sign outgoing messages, and another to validate the signature on incoming messages.

downloadbutton

## Running the sample

Running the sample will result in three different NServiceBus message endpoints, each running within their own console window:

1. **Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint**: Receives messages from the other two endpoints, but only successfully processing messages with a valid signature.
1. **Samples.Pipeline.SigningAndEncryption.SignedSender**: Sends messages to the ReceivingEndpoint with a proper signature.
1. **Samples.Pipeline.SigningAndEncryption.UnsignedSender**: Sends messages to the ReceivingEndpoint with a fake, invalid signature.

Pressing any key from the **SignedSender** window will result in a message sent to the **ReceivingEndpoint** that is successfully processed:

```
INFO  MyMessageHandler Handling message...
INFO  MyMessageHandler   Contents = d98b7529-f2b8-460d-9798-e1dfacda930b
INFO  MyMessageHandler   Signature = UB+cX5u15CTtsAPBtECbpi81hDSr9PiqebBmpzxp81k=
```

On the other hand, pressing any key from the **UnsignedSender** window will result in a message sent to the **ReceivingEndpoint** that is rejected because of its fake signature:

```
ERROR SignatureVerificationBehavior Message signature for message id 333d2bb7-3671-47d5-9b00-aad9016905c4 is invalid. The message will be discarded.
```


## Code walkhrough

The **Shared** project contains the behaviors that implement message signing and signature verification, as well as an extension method that makes it easy to register the behaviors within an endpoint. The other three projects are the NServiceBus message endpoints described above, which refer to the items in the **Shared** project


### MessageSigningBehavior

The `MessageSigningBehavior` operates on the `IOutgoingPhysicalMessageContext`, which gives access to the outgoing message after it has been serialized to bytes. The behavior calculates the HMACSHA256 hash of the message bytes, and adds the Base64-encoded value as a message header to the outgoing message.

snippet: MessageSigningBehavior

The call to `next()` invokes the remainder of the outgoing pipeline, eventually resulting in the message being dispatched to the message transport.

NOTE: While this sample shows how to do message signing, the same pattern could be used to implement full encryption of the message body. In this case, a message signature would not be written to the message headers. Instead, the entire body of the message would be replaced using `IOutgoingPhysicalMessageContext`'s `UpdateMessage(byte[] newBody)` method.


### SignatureVerificationBehavior

The `SignatureVerificationBehavior` operates on the `IIncomingPhysicalMessageContext`, which gives access to the serialized message, but on the incoming message. It also calculates the correct HMACSHA256 hash, and only calls `next()` to invoke the rest of the incoming pipeline (which includes the message handler) if the message signature is present and is correct.

snippet: SignatureVerificationBehavior

In this implementation, returning `Task.CompletedTask` without calling `next()` effectively stops the message processing pipeline, as further behaviors (including executing the message handlers) are not executed. This results in the message being consumed with an `ERROR` log message, but no exception thrown.

WARNING: Consuming the message in this manner effectively deletes it. If an upstream message source were to begin accidentally signing messages incorrectly, the message would still be consumed, which would result in message loss. It is probably a good idea to audit these messages in some way.

Another strategy would be to throw an exception, rather than consuming the message. This would result in the message going to the error queue, which would prevent message loss. However, this would first result in repeated retry attempts, none of which would succeed with an invalid message signature. This could be avoided by using a custom exception type (i.e. `MessageSignatureInvalidException`) and then configuring that message type to be an [unrecoverable exception](/nservicebus/recoverability/#unrecoverable-exceptions).


### Configuring the behaviors

The behaviors to sign outgoing messages and verify the signatures on incoming messages must both be added to the NServiceBus pipeline. Bundling this into an extension method enables configuring an endpoint to activate message signing with one line of code.

snippet: BehaviorConfigExtension

Both the **SignedSender** and **ReceivingEndpoint** call this extension method, while the **UnsignedSender** does not.

snippet: EnableSigning

## Summary

The extensibility offered by the NServiceBus pipeline easily allows plugging in behaviors that enable message signing and verification. While this sample shows message signing, the same pattern could be leveraged to provide full end-to-end encryption of the message body.

Rather than trying to configure all the different options (cipher selection, key storage, key rotation, etc.) on a one-size-fits-all solution, implementing an infrastructure solution with behaviors requires only the minimum amount of code necessary to fit a system's exact nonfunctional requirements.