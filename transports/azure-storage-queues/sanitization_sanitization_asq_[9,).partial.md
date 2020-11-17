In Versions 9.0 and above, the Azure Storage Queues transport no longer sanitizes queues. Queue names should follow Azure Storage Queues naming rules.


## Azure Storage Queues naming rules

 1. A queue name must start with a letter or number, and can only contain letters, numbers, and the dash (`-`) character.
 1. The first and last letters in the queue name must be alphanumeric.
 1. The dash (`-`) character cannot be the first or last character.
 1. Consecutive dash characters are not permitted in the queue name.
 1. All letters in a queue name must be lowercase.
 1. A queue name must be between 3 and 63 characters long.


## Backward compatibility with versions 7 and below

To remain backward compatible with endpoints versions 8 and below, endpoints version 9 and above should be configured with proper queue names. To provide queue names that follow the old version sanitization rules, the following custom code can be used to ensure queues are named in a backwards compatible manner.

Sanitization code for MD5 and SHA1:

snippet: azure-storage-queue-backwards-compatible-sanitization


## Future consideration prior to using sanitization

Things to consider:

 * Truncated long queue names could conflict.
 * Hashed queue names could lead to difficult names to use during production troubleshooting or debugging.
 * Sanitized queue names stay in the system and cannot be replaced until no longer used.