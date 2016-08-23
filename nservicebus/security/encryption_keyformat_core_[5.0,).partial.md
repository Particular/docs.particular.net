
#### Key format

The key format can be specified in either *Base64* or *ASCII* format.

With ASCII its not possible to use the full 8 bit range of a byte as its a 7 bit encoding and even then some characters need to be escaped which is not done resulting in even less characters. Meaning per byte only about 100 values are used. When using 16 byte / 128 bit keys this means only about 100^16 combinations are possible versus 256^16.

NOTE: Use Base64 whenever possible, ASCII 7 bit keys are meant for backwards compatibility.