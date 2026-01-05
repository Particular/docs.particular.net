> [!NOTE]
> If there are multiple saga types that handle a given message type and one of them is found while others are not, the saga not found handlers **will not be executed**. The saga not found handlers are executed only if no saga instances are invoked. The following table illustrates when the saga not found handlers are invoked when a message is mapped to two different saga types, A and B.

| Saga A found | Saga B found | Not found handler invoked |
|--------|--------|---------|
| ✔️    | ✔️     | ❌     |
| ✔️    | ❌     | ❌     |
| ❌    | ✔️     | ❌     |
| ❌    | ❌     | ✔️     |