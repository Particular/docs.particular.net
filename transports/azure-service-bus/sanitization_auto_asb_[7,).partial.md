By default, the transport uses the `ThrowOnFailedValidation` sanitization strategy. This strategy allows sanitization rules to be specified that remove invalid characters and hashing algorithm to shorten entity path/name that is exceeding maximum length. For an invalid entity path/name, an exception is thrown. Validation rules can be adjusted by providing custom validation rules per entity type.

snippet: asb-ThrowOnFailedValidation-sanitization-overrides

Where `ValidationResult` provides the following

 * Characters are valid or not
 * Length is valid or not

To customize sanitization for some of the entities, `ValidateAndHashIfNeeded` strategy can be used. This strategy allows to specify sanitization rules to remove invalid characters and hashing algorithm to shorten entity path/name that is exceeding maximum length.

NOTE: `ValidateAndHashIfNeeded` is using validation rules to determine what needs to be sanitized. First step, invalid characters are removed. Second step, hashing is applied if length is still exceeding the maximum allowed length.

snippet: asb-ValidateAndHashIfNeeded-sanitization-overrides

In cases where an alternative sanitization is required, a custom sanitization can be applied.

snippet: asb-custom-sanitization

Custom sanitization strategy definition:

snippet: asb-custom-sanitization-strategy

If the implementation of a sanitization strategy requires configuration settings, these settings can be accessed using [dependency injection](/nservicebus/dependency-injection/) to access an instance of `ReadOnlySettings`.

snippet: custom-sanitization-strategy-with-settings


### Backward compatibility with versions 6 and below

To remain backward compatible with endpoints versions 6 and below, endpoints version 7 and above should be configured to perform sanitization based on version 6 and below rules. The following custom sanitization will ensure entities are sanitized in a backwards compatible manner.

snippet: asb-backward-compatible-custom-sanitiaztion-strategy
