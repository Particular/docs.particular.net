---
title: Encryption with Multi-Key Decryption
summary: How to enable encryption with multi-key decryption.
tags:
- Encryption
---

**NOTE**: This article refers to NServiceBus V4 and V5

When original encryption key is replaced by a new encryption key, messages in-flight that were encrypted with original key will fail decryption, unless original encryption key is added to a list of expired keys. You can configure NServiceBus to use multiple expired keys. Expired keys are used solely for decryption.

Multi-key decryption configured through the Rijndael encryption service section in configuration file:

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

and enabled through endpoint configuration

#### NServiceBus Version 4

<!-- import RijndaelEncryptionServiceSimpleV4 -->

#### NServiceBus Version 5

```c#
var configuration = new BusConfiguration();
configuration.RijndaelEncryptionService();
```

### Rijndael Encryption From Custom IProvideConfiguration

Alternative to configuration file is encryption with multi-key decryption configured by implementation of ```IProvideConfiguration<RijndaelEncryptionServiceConfig>```. This approach can be used for shared configuration via code and unit testing (to supply various configuration permutations).

#### NServiceBus Version 4 and 5

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
