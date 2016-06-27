---
title: Generating secure random strong encryption keys
summary: Describes options in generating secure random strong encryption keys
tags:
- Encryption
- Security
redirects:
related:
- nservicebus/security
- nservicebus/security/encryption
---

There are multiple ways of generating a key. Most implementations rely on a *random* object. All examples mentioned here use a secure cryptographic randomizer.


### Powershell


#### Base64

```ps
[Reflection.Assembly]::LoadWithPartialName("System.Security")
$rijndael = new-Object System.Security.Cryptography.RijndaelManaged
$rijndael.GenerateKey()
Write-Host([Convert]::ToBase64String($rijndael.Key))
$rijndael.Dispose()
```

#### Hex

```ps
[Reflection.Assembly]::LoadWithPartialName("System.Security")
$rijndael = new-Object System.Security.Cryptography.RijndaelManaged
$rijndael.GenerateKey()
Write-Host([System.BitConverter]::ToString($rijndael.Key).Replace("-", "").ToLowerInvariant())
$rijndael.Dispose()
```

### c# ###

The code snippets below can be run from Linqpad or by copy and pasting the code in a new project and referencing `System.Security`.


#### Base64

```cs
using (var e = System.Security.Cryptography.RijndaelManaged.Create())
{
	e.GenerateKey();
    Console.WriteLine(Convert.ToBase64String(e.Key));
}
```


#### Hex

```cs
using (var e = System.Security.Cryptography.RijndaelManaged.Create())
{
	e.GenerateKey();
    Console.WriteLine(BitConverter.ToString(e.Key).Replace("-", string.Empty).ToLowerInvariant());
}
```


### OpenSSL

OpenSSL is well known for its ability to generate certificates but it can also be used to generate random data.


#### Base64

Generates 32 random bytes (256bits) in a base64 encoded output:

```dos
openssl rand -base64 32
```


#### Plaintext

Generates 32 random characters (256bits):

```dos
openssl rand 32
```

NOTE: Be aware that the string parsed by NServiceBus do not use extended ASCII which limits the key range to 7 bits per character.


### CryptoKeyGenerator

A key generator exists in ParticularLabs that uses the .NET framework crypto provider to generate a key.

Download the [CryptoKeyGenerator](https://github.com/ParticularLabs/CryptoKeyGenerator) labs project, build it and run it and copy paste the random key in its correct format.


After running the tool it generates one key and outputs this key in multiple formats.

Output

```no-highlight
Strip 8th bit: True
Strip control: True
Key bit length: 256

Base64:
        KzpSTk1pezg5eTJRNmhWJmoxdFo6UDk2WlhaOyQ5N0U=
        |---------||---------||---------||---------|

Hex:
        2b3a524e4d697b3839793251366856266a31745a3a5039365a585a3b24393745
        |--------------||--------------||--------------||--------------|

ASCII:
        +:RNMi{89y2Q6hV&j1tZ:P96ZXZ;$97E     xml escape: +:RNMi{89y2Q6hV&amp;j1tZ:P96ZXZ;$97E
        |------||------||------||------|

ASCII-EX:
        +:RNMi{89y2Q6hV&j1tZ:P96ZXZ;$97E     xml escape: +:RNMi{89y2Q6hV&amp;j1tZ:P96ZXZ;$97E
        |------||------||------||------|
```