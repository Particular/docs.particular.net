When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

#### Starting, updating or deleting saga data

Azure Table Persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
Azure.Data.Tables.TableTransactionFailedException: The update condition specified in the request was not satisfied.
RequestId:f0742973-2002-0060-09d3-f9afb2000000
Time:2022-11-16T15:53:36.0074148Z
Status: 412 (Precondition Failed)
ErrorCode: UpdateConditionNotSatisfied
```