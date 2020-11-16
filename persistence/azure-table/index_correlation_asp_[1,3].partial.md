NOTE: In Versions 6 and above of NServiceBus, all correlated properties are [unique by default](/nservicebus/upgrades/5to6/handlers-and-sagas.md#saga-api-changes-unique-attribute-no-longer-needed) so there is no longer a configuration setting.

One of the limitations of the Azure Storage Persistence is support for only one correlation property.