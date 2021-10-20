## Fluent-style API is not supported

Starting with NServiceBus version 8, the fluent-style testing API is not supported. Tests should be written in an [Arrange-Act-Assert (AAA) style](https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics#write-your-tests). Tests written this way will create the handler or saga to be tested, and call methods on them directly, passing in a testable message handler context that will capture outgoing operations that can be asserted on afterwards.

See the [upgrade guide](/nservicebus/upgrades/testing-7to8.md) for details on converting existing tests.
