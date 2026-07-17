* **Rule ID**: NSB0004
* **Severity**: Error
* **Example message**: This saga mapping expression must be rewritten using `mapper.MapSaga(…).ToMessage<T>(…)` syntax. Use the code fix to transition to the new syntax.
include: simplify-warning-description

Starting with Version 10, NSB0018 has been merged into NSB0004. The combined diagnostic is always reported as an Error.