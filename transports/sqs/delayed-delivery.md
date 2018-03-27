---
title: Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQS transport
component: Sqs
reviewed: 2018-03-26
versions: '[4,]'
---

In Versions 4 and above, the SQS transport supports [delayed delivery](/nservicebus/messaging/delayed-delivery.md) of messages longer than 15 minutes (900 seconds).

## Enable unrestricted delayed delivery

The unrestricted delayed delivery has to be enabled on the transport configuration:

snippet: DelayedDelivery

Unrestricted delayed delivery needs to be enabled on the sender and receiver to be able to delay messages longer than 900 seconds.

| Scenario                    | Sender   | Receiver | Supported     |
|-----------------------------|----------|----------|:-------------:|
| delay duration <= 900 sec   | disabled | disabled | Yes           |
|                             | disabled | enabled  | Yes           |
|                             | enabled  | disabled | Yes           |
|                             | enabled  | enabled  | Yes           |
| delay duration > 900 sec    | disabled | disabled | No            |
|                             | disabled | enabled  | No            |
|                             | enabled  | disabled | No            |
|                             | enabled  | enabled  | Yes           |

WARNING: As the chart indicates, sending messages with a delay duration longer than 900 seconds to endpoints using Versions 3 and below is not supported.

Unrestricted delayed delivery requires a FIFO queue for each endpoint that receives delayed messages. The transport handles creation of the FIFO queue automatically when [installers](/nservicebus/operations/installers.md) are enabled.

### Manual FIFO queue creation

If installers are not used, then the FIFO queue will need to be manually created.

The FIFO queue has the following requirements:

- The name must match the endpoint's input queue suffixed with `-delay.fifo`.
- The Delivery Delay setting (DelaySeconds) should be set to 900 seconds.
- The Message Retention Period should be set to at least 4 days.
- A Redrive Policy must not be configured.

For an example of how to manually create queues, see [scripting](/transports/sqs/operations-scripting.md).


## How it works

When a delayed message is sent, the delay duration is calculated. If it's less than or equal to 900 seconds, the message is sent directly to the destination input queue with the `DelaySeconds` message attribute set to the delay duration.

If the delay duration is greater than 900 seconds, then the message is sent to the destination's FIFO queue with the `NServiceBus.AmazonSQS.DelaySeconds` custom message attribute set to the delay duration. When the message is received from the FIFO queue after 900 seconds, the remaining delay duration is calculated. If it's less or equal to 900 seconds, the message is forwarded to the destination input queue with the `DelaySeconds` message attribute set to the remaining delay duration. Otherwise, the message is sent back to the FIFO queue with an updated custom message attribute set to the remaining delay duration.

The following sequence diagram illustrates a message sent with a delay duration greater than 900 seconds:

```mermaid
sequenceDiagram
    participant S as Sender
    participant F as Destination-delay.fifo
    participant D as Destination
    
    S ->> F:  NServiceBus.AmazonSQS.DelaySeconds = delay
    loop every 900sec
        alt remaining delay > 900sec
            F -->> F:  NServiceBus.AmazonSQS.DelaySeconds = remaining delay
        else remaining delay <= 900sec
            F ->> D: DelaySeconds = remaining delay
        end
    end
```

### Clock drift

To avoid clock drift, the broker timestamps are used wherever possible to calculate the remaining timeout. The due time calculation uses `SentTimestamp` as well as `ApproximateFirstReceiveTimestamp` set by the broker. Only in cases of re-delivery when `ApproximateReceiveCount` is higher than one the client's clock is used and thus subjected to clock drift.

### Delivery

For unrestricted delayed deliveries, the last step is always a handover from the FIFO queue to the endpoint's input queue. SQS does not provide cross queue operation transactions, so the handover is subjected to retries. In cases of retries, it might be possible that timeouts are delivered more than once. Message handlers need to be idempotent when used with transports with [transaction](/transports/transactions.md) level `Receive Only` or below. The following diagram illustrates that:

```mermaid
sequenceDiagram
    participant D as Destination
    participant F as Destination-delay.fifo
    F ->>+ F: Timeout due
    F ->> D: Send with remaining delay
    Note left of D: Original message
    F ->>- F: Delete Delayed Message failed
    Note right of F: Network outage
    F ->>+ F: Timeout due
    F ->> D: Send with remaining delay
    F ->>- F: Delete Delayed Message
    Note left of D: Duplicate message
```

### Example

Below is an example of a delayed delivery less or equal to 900 seconds:

```mermaid
graph LR

subgraph Example: delay of 14 min and 5 seconds

sender
destination

sender .-> |T1: Delay with 845sec| destination
destination --> |"T2: fa:fa-hourglass-half 845sec"| destination

end
```

14 min and 4 seconds are in total 845 seconds. This is less than 900 seconds, so the message will be directly sent to the destination with a `DelaySeconds` value of 845 seconds. No message attribute header will be used.

Below is an example of a delayed delivery greater than 900 seconds:

```mermaid
graph LR

subgraph Example: delay of 32 min and 5 seconds

sender
fifo(destination-delay.fifo)
destination

sender .-> |T1: Delay with 1,925sec| fifo
fifo --> |"T2: fa:fa-hourglass-half Delay with 900sec"| fifo
fifo --> |"T3: fa:fa-hourglass-half Delay 900sec"| fifo
fifo .-> |"T3: Send to destination"| destination
destination --> |"T4: fa:fa-hourglass-half Delay with 125sec"| destination

end
```

32 min and 5 seconds are in total 1,925 seconds. This will lead to two 900 seconds cycles on the FIFO queue and one delayed delivery on the destination queue with the remaining timeout of 125 seconds (handover between FIFO queue and input queue).

## Cost considerations

Enabling unrestricted delayed delivery will have an impact on cost because FIFO queues are required.

To estimate the cost of a delayed message, the following formula can be used:

**N** = delay in seconds<br>
**P** = price per request<br>
**C**(ycles) = **N** / 900<br>
**O**(perations) = **C** * 2 // dequeue and requeue<br>
**T**(otal cost) = **O** * **P**<br>

NOTE: The cost might be lower due to the transport optimizing dequeue operations by batching requests.

### Example

To calculate the cost of a single message delayed for a year, the following applies:

[Price per 1 Million Requests after Free Tier (Monthly)](https://aws.amazon.com/sqs/pricing/)

FIFO Queue: $0.50 ($0.00000050 per request)

N = 31,536,000 seconds<br>
P = $0.00000050<br>
C = 31,536,000 / 900 = 35,040<br>
O = 35,040 * 2 = 70,080<br>
T = 70,080 * $0.00000050 = $0.03504<br>