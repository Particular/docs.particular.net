
The tests have to follow a well-defined structure otherwise subtle issues may arise at runtime. The recommended structure is presented in the code snippets in this article.

`Test.Initialize()` (or any of its overloads) must be the first call in _all_ test methods.

NOTE: Expectations should generally be specified before invoking the tested behaviour (the exception is testing saga timeouts).

If [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md) is used, the conventions must be configured in the `Test.Initialize()` method:

snippet: SetupConventionsForUnitTests
