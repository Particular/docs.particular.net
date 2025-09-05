### Payload signing

snippet: DisablePayloadSigning

Amazon S3 requires the payload to be signed when uploaded to the S3 bucket. The SQS transport allows disabling the payload signing by setting the `DisablePayloadSigning` to true to enable support for alternate storages, such as [Cloudflare R2](https://www.cloudflare.com/developer-platform/products/r2/).
