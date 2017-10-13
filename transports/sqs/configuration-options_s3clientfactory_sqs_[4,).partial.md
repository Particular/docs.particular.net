## S3 Client Factory

**Optional**

**Default**: `() => new AmazonS3Client()`.

This overloads the default S3 client factory with a custom factory creation delegate.

**Example**: To use a custom factory, specify:

snippet: S3ClientFactory