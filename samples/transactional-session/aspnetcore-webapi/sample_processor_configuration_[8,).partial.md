

#### Sanitization

One of the endpoints is using a long name which needs to be sanitized. 

snippet: endpointname

To remain backwards compatible with the older versions of the transport, `MD5` based sanitization is registered. The sample also includes `SHA1` based sanitization. This sanitizer is suitable for endpoints with the transport version 7.x used to shorten queue names with `SHA1` hashing algorithm.

snippet: sanitization

The full contents of the sanitization code is shown at the [end of this document](#sanitizer-source-code).