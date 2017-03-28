
The tests have to follow a well-defined structure, otherwise a subtle issues might arise at runtime. The recommended structure is presented in the code snippets in this article.

Before executing any test method, it is necessary to call the `Test.Initialize()` method (or any of its overloads).

NOTE: The tests are written using fluent API, but expectations should be generally specified before invoking the tested behaviour (the exception is testing saga timeouts).

If [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md) is used, the conventions must be configured in the `Test.Initialize()` method:

snippet: SetupConventionsForUnitTests
