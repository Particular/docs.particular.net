## S3 Client Factory

**Optional**

**Default**: `() => new AmazonS3Client()`

This overloads the default S3 client factory with a custom factory creation delegate.

**Example**: To use a custom factory, specify:

snippet: S3ClientFactory

### ServerSideEncryption

**Optional**

**Default**: Null

Specifies the server-side encryption method and an optional key management service key ID to be used when storing large message bodies on S3. If this option is specified in addition to server-side customer encryption, an exception will be thrown.

snippet: S3ServerSideEncryption

### ServerSideCustomerEncryption

**Optional**

**Default**: Null

Specifies the server-side customer encryption method, the encryption key and an optional MD5 of the provided key to be used when storing and retrieving large message bodies using S3. If this option is specified with an empty key or in addition to server-side encryption, an exception will be thrown.

snippet: S3ServerSideCustomerEncryption
