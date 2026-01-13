## Manual saga registration

By default, sagas are automatically discovered through [assembly scanning](/nservicebus/hosting/assembly-scanning.md?version=core_10). When [assembly scanning is disabled](/nservicebus/hosting/assembly-scanning.md?version=core_10#disable-assembly-scanning), sagas must be manually registered using `AddSaga<TSaga>()`:

snippet: RegisterSagaManually

