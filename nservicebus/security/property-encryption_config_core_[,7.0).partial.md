
### Via App.config

Warning: Configuration of encryption keys via `app.config` and `IProvideConfiguration` is not recommended in Version 6, use the [code configuration API](#configuration-via-code) instead. The configuration section will be removed in Version 7.

The encryption key can be defined in the `app.config`.

snippet: EncryptionFromAppConfig


### Via IProvideConfiguration

snippet:EncryptionFromIProvideConfiguration

For more info on `IProvideConfiguration` see [Customizing NServiceBus Configuration](/nservicebus/hosting/custom-configuration-providers.md)