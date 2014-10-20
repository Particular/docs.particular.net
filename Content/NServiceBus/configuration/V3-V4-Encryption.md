---
title: Configuration API Encryption in V3 and V4
summary: Configuration API Encryption in V3 and V4
tags:
- NServiceBus
- BusConfiguration
- V3
- V4
---

NServiceBus can encrypt message properties. To configure the encryption feature you must define the encryption algorithm. NServiceBus supports Rijndael out of the box and you can configure it by calling the `RijndaelEncryptionService()` method of the `Configure` instance.

* [Encryption Sample](/nservicebus/encryption-sample)
* [Encryption with Multi-Key Decryption](/nservicebus/encryption-with-multi-key-decryption)