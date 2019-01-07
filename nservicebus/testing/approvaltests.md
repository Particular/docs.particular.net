---
title: Testing NServiceBus with ApprovalTests
summary: Verify NServiceBus test contexts using ApprovalTests.
reviewed: 2019-01-07
component: ApprovalTests
---


`NServiceBus.ApprovalTests` adds support for using [ApprovalTests](https://github.com/approvals/ApprovalTests.Net) to verify [NServiceBus Test Contexts](/samples/unit-testing/).


## Verifying a context

Given the following handler:

snippet: SimpleHandler

The test that verifies the resulting context:

snippet: HandlerTest

The resulting context verification file is as follows:

```json
{
  "RepliedMessages": [
    {
      "MyReplyMessage": {
        "Property": "Value"
      }
    }
  ],
  "ForwardedMessages": [
    "newDestination"
  ],
  "MessageId": "Guid 1",
  "ReplyToAddress": "reply address",
  "SentMessages": [
    {
      "MySendMessage": {
        "Property": "Value"
      },
      "Options": {
        "DeliveryDelay": "12:00:00"
      }
    }
  ],
  "PublishedMessages": [
    {
      "MyPublishMessage": {
        "Property": "Value"
      }
    }
  ],
  "Extensions": {}
}
```


## Example behavior change

The next time there is a code change, that results in a different resulting interactions with NServiceBus, those changes can be visualized. For example if the `DelayDeliveryWith` is changed from 12 hours to 1 day:

snippet: SimpleHandlerV2

Then the resulting visualization diff would look as follows:


![visualization diff](approvaltests-diff.png)