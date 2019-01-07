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