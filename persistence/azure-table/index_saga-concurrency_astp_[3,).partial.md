When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

#### Starting, updating or deleting saga data

Azure Table Persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
Microsoft.WindowsAzure.Storage.StorageException: Element 0 in the batch returned an unexpected response code.

Request Information
RequestID:010c234e-3002-0145-06eb-72b85a000000
RequestDate:Tue, 24 Sep 2019 15:16:45 GMT
StatusMessage:The update condition specified in the request was not satisfied.
ErrorCode:
ErrorMessage:The update condition specified in the request was not satisfied.
RequestId:010c234e-3002-0145-06eb-72b85a000000
Time:2019-09-24T15:16:46.0746310Z
```