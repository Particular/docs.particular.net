### SqlSagaAttribute

Sagas must be decorated with a `[SqlSagaAttribute]`. If no [saga finder](/nservicebus/sagas/saga-finding.md) is defined, the `correlationProperty` must match the [correlated saga property](/nservicebus/sagas/message-correlation.md).