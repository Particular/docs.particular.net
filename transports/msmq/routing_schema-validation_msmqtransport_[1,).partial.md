## Schema validation

Every time the instance mapping file is loaded, it is validated against a schema to ensure it is correct. In NServiceBus.Transport.Msmq version 1.0 this schema allows arbitrary attributes in the `instance` element. NServiceBus.Transport.Msmq version 1.1 introduced a stricter schema which checks to ensure that no unexpected attributes are present. This helps to discover common mistakes with the instance mapping file, such as misspelled attributes.

If the instance mapping file cannot be validated against the stricter schema, the endpoint logs a warning and falls back to the old schema validation mechanism.

NOTE: Schema validation fallback is only logged the first time it happens. Subsequent fallbacks are handled silently.

The transport can be configured to throw an error if the instance mapping file fails the strict schema validation, rather than falling back:

snippet: strict-schema-validation