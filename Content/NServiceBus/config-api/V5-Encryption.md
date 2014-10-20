---
title: Configuration API Encryption in V5
summary: Configuration API Encryption in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

To configure the encryption feature you must define the encryption algorithm. NServiceBus supports Rijndael out of the box and you can configure it by calling the `RijndaelEncryptionService()` method. If you need to plugin your own encryption service you can invoke the `RegisterEncryptionService()` method, of the `BusConfiguration` class, that accepts a delegate that will be invoked at runtime when an instance of the current `IEncryptionService` is required.

* [Encryption Sample](encryption-sample).
* [Encryption with Multi-Key Decryption](encryption-with-multi-key-decryption.md)

	//TODO: Define encrypted properties how to?