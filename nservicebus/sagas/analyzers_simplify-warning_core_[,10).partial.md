* **Rule ID**: NSB0004, NSB0018
* **Severity**: Warning (NSB0004), Info (NSB0018)
* **Example message**: The saga mapping contains multiple .ToSaga(…) expressions which can be simplified using `mapper.MapSaga(…).ToMessage<T>(…)` syntax.
include: simplify-warning-description

The diagnostics NSB0004 and NSB0018 are the same, but are applied in different contexts. There is no duplication when only one mapping expression exists, so NSB0018 is presented at level Info. When two or more mapping expressions exist, duplication is present, so NSB0004 is presented as a warning.
