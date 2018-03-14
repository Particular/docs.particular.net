---
title: Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQS transport
component: Sqs
reviewed: 2018-03-02
versions: '[4,]'
---

In Versions 4.0 and above, the SQS transport allows [delayed delivery](/nservicebus/messaging/delayed-delivery.md) of messages longer than 15 minutes. The transport creates a FIFO queue per endpoint that allows delaying messages for longer periods of time.

## How it works

```mermaid
sequenceDiagram
    participant S as Sender
    participant D as Destination
    participant F as Destination-delay.fifo
    alt delay <= 900sec
        S ->> D: Message DelaySecond = delay
    else delay > 900sec
        S -->> F:  Attribute DelaySecond = delay
        loop every 900sec
           alt remaining delay <= 900sec
              F ->> D: Message DelaySecond = remaining delay
           else remaining delay> 900sec
              F -->> F:  Attribute DelaySecond = remaining delay
           end
        end
end
```


### Delay levels


### Delivery

### Example

```mermaid
graph LR

subgraph Example: delay of 14 min and 5 seconds

sender
destination

sender .-> |T1: Delay with 845sec| destination
destination --> |"T2: fa:fa-hourglass-half 845sec"| destination

end
```

```mermaid
graph LR

subgraph Example: delay of 32 min and 5 seconds

sender
fifo(destination-delay.fifo)
destination

sender .-> |T1: Delay with 1925sec| fifo
fifo --> |"T2: fa:fa-hourglass-half Delay with 900sec"| fifo
fifo --> |"T3: fa:fa-hourglass-half Delay 900sec"| fifo
fifo .-> |"T3: Send to destination"| destination
destination --> |"T4: fa:fa-hourglass-half Delay with 125sec"| destination

end
```

## Backwards compatibility

