## Manual saga registration

By default, sagas are automatically discovered through [assembly scanning](/nservicebus/hosting/assembly-scanning.md). When [assembly scanning is disabled](/nservicebus/hosting/assembly-scanning-disable.md), sagas must be manually registered using `AddSaga<TSaga>()`:

snippet: RegisterSagaManually


