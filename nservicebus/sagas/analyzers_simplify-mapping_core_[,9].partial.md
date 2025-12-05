## Saga mapping expressions can be simplified

* **Rule ID**: NSB0004, NSB0018
* **Severity**: Warning (NSB0004), Info (NSB0018)
* **Example message**: The saga mapping contains multiple .ToSaga(…) expressions which can be simplified using mapper.MapSaga(…).ToMessage<T>(…) syntax.

The original NServiceBus saga mapping API required repeating the `.ToSaga(…)` expressions for each call to `.ConfigureMapping(…)`.

The IDE will raise a diagnostic for mapping expressions like this:

snippet: SagaAnalyzerComplexMapping

The analyzer will also offer a code fix that will automatically rewrite the code to look like this:

snippet: SagaAnalyzerSimplifiedMapping

The simplified syntax removes duplication and reduces confusion since the `.ToSaga(…)` mappings in the old syntax must match to be valid.

The diagnostics NSB0004 and NSB0018 are the same but with different severity in different contexts. There is no duplication when only one mapping expression exists, so NSB0018 is presented at level Info. When two or more mapping expressions exist, duplication is present, so NSB0004 is presented as a Warning.

## Saga can only define a single correlation property on the saga data

* **Rule ID**: NSB0005
* **Severity**: Error
* **Example message**: The saga can only map the correlation ID to one property on the saga data class.

When using the `.ConfigureMapping<T>(…).ToSaga(…)` mapping pattern, all of the `.ToSaga(…)` expressions must agree and point to the same property on the saga data class.

This was a runtime error in NServiceBus 7.6 and below, but the analyzer raises the error as more direct feedback at compile time.

Once all the `.ToSaga(…)` expressions agree, [NSB0004: Saga mapping expressions can be simplified](#saga-mapping-expressions-can-be-simplified) will be invoked, and the code fix can be used to simplify the saga mapping expression so that `.ToSaga(…)` mappings are not duplicated.
