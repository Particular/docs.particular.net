## Saga can only define a single correlation property on the saga data

* **Rule ID**: NSB0005
* **Severity**: Error
* **Example message**: The saga can only map the correlation ID to one property on the saga data class.

When using the `.ConfigureMapping<T>(…).ToSaga(…)` mapping pattern, all of the `.ToSaga(…)` expressions must agree and point to the same property on the saga data class.

This was a runtime error in NServiceBus 7.6 and below, but the analyzer raises the error as more direct feedback at compile time.

Once all the `.ToSaga(…)` expressions agree, [NSB0004: Saga mapping expressions can be simplified](#saga-mapping-expressions-can-be-simplified) will be invoked, and the code fix can be used to simplify the saga mapping expression so that `.ToSaga(…)` mappings are not duplicated.
