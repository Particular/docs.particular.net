* **Example message**: The saga mapping contains multiple .ToSaga(…) expressions which can be simplified using mapper.MapSaga(…).ToMessage<T>(…) syntax.

The original NServiceBus saga mapping API required repeating the `.ToSaga(…)` expressions for each call to `.ConfigureMapping(…)`.

The IDE will raise a diagnostic for mapping expressions like this:

snippet: SagaAnalyzerComplexMapping

The analyzer will also offer a code fix that will automatically rewrite the code to look like this:

snippet: SagaAnalyzerSimplifiedMapping

The simplified syntax removes duplication and reduces confusion since the `.ToSaga(…)` mappings in the old syntax must match to be valid.