---
title: Encryption with Multi-Key Decryption
summary: How to enable encryption with multi-key decryption.
tags:
- Encryption
---

**NOTE**: This article refers to NServiceBus V4 and V5.

When the original encryption key is replaced by a new encryption key, in-flight messages that were encrypted with the original key will fail decryption unless the original encryption key is added to a list of expired keys. You can configure NServiceBus to use multiple expired keys. Expired keys are used solely for decryption.

You can configure multi-key decryption through the Rijndael encryption service section in the configuration file:

```xml
<configuration>
    <configSections>
        <section name="RijndaelEncryptionServiceConfig" 
                 type="NServiceBus.Config.RijndaelEncryptionServiceConfig, NServiceBus.Core" />
	</configSections>
	<RijndaelEncryptionServiceConfig Key="gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6">
  		<ExpiredKeys>
    		<add Key="abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" />
    		<add Key="cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" />
  		</ExpiredKeys>
	</RijndaelEncryptionServiceConfig>
</configuration>
```

Enable the multi-key decryption through endpoint configuration.

#### NServiceBus V4

<!-- import RijndaelEncryptionServiceSimpleV4 -->

#### NServiceBus V5

```c#
var configuration = new BusConfiguration();
configuration.RijndaelEncryptionService();
```

### Rijndael Encryption From Custom IProvideConfiguration

An alternative to using the configuration file is to implement ```IProvideConfiguration<RijndaelEncryptionServiceConfig>```. Use this approach for shared configuration via code and unit testing (to supply various configuration permutations).

#### NServiceBus V4 and V5

```c#
public class ConfigureEncryption : IProvideConfiguration<RijndaelEncryptionServiceConfig>
{
	public RijndaelEncryptionServiceConfig GetConfiguration()
    {
        return new RijndaelEncryptionServiceConfig 
					{ 
						Key = "gdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
						ExpiredKeys = new RijndaelExpiredKeyCollection
                		{
                    		new RijndaelExpiredKey { Key = "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" },
		                    new RijndaelExpiredKey { Key = "cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6" }
						}
					};
    }
}
```
