## Fluent-style API is not supported

The fluent-style testing API is not supported. Write tests using the [Arrange-Act-Assert (AAA) style](https://learn.microsoft.com/en-us/visualstudio/test/unit-test-basics#write-your-tests). Create the handler or saga and call its methods directly, passing in a testable message handler context. The context captures outgoing operations for assertions.

See the [unit testing sample](/samples/unit-testing/) for examples and the [upgrade guide](/nservicebus/upgrades/testing-7to8.md) for help converting existing tests.
