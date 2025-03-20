For example, for the `ValidateAndHashIfNeeded` strategy, the sanitization functions must include the strategy logic to preserve the same entity names.

For queue names, or event names that crossed the threshold of 50 characters, it is necessary to precalculate the MD5 hash and store that as the subscription or rule name. Alternatively simply configure the subscription or rule name already used in production as a hardcoded value.

snippet: asb-sanitization-compatibility
