### SqlSagaAttribute

Sagas need to be decorated with a `[SqlSagaAttribute]`. If no [Saga Finder](/nservicebus/sagas/saga-finding.md) is defined then the `correlationProperty` needs to match the [Correlated Saga Property](/nservicebus/sagas/message-correlation.md).