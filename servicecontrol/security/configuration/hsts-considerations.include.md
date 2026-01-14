### HSTS Considerations

- HSTS should not be tested on localhost because browsers cache the policy, which could break other local development
- HSTS is disabled in Development environment (ASP.NET Core excludes localhost by default)
- HSTS can be configured at either the reverse proxy level or in ServiceControl (but not both)
- HSTS is cached by browsers, so test carefully before enabling in production
- Start with a short max-age during initial deployment
- Consider the impact on subdomains before enabling `includeSubDomains`