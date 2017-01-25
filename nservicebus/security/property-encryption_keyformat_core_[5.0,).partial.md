
#### Key format

The key format can be specified in either *Base64* or *ASCII* format.

With ASCII its not possible to use the full 8 bit range of a byte as it is a 7 bit encoding and even then some characters need to be escaped which is not done resulting in even less characters. Only about 100 values per byte are used. For 16 byte (128 bit) keys, this means that only about 100^16 out of all available 256^16 combinations are used.

NOTE: Use Base64 whenever possible. ASCII 7 bit key format should only be used for backwards compatibility.