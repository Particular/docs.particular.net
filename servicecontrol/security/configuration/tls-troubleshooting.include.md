### Certificate not found or access denied

**Symptom**: The application fails to start with certificate-related errors when using a PFX certificate file.

**Cause**: The certificate file path is incorrect, the file is not readable, or the password is wrong.

**Solutions**:

- Verify the certificate path is correct and the file exists
- Check that the certificate file has appropriate permissions
- Confirm the certificate password is correct
- For containers, ensure the volume mount is correct (e.g., `-v certificate.pfx:/usr/share/ParticularSoftware/certificate.pfx`)

### HTTPS redirect not working

**Symptom**: HTTP requests are not redirected to HTTPS when using a reverse proxy.

**Cause**: The redirect requires the `X-Forwarded-Proto` header to detect the original protocol, which may not be processed.

**Solutions**:

- Verify `RedirectHttpToHttps` is set to `true`
- Ensure the HTTPS port setting is configured correctly
- Configure [forward headers](forward-headers.md) so the `X-Forwarded-Proto` header is trusted
- Confirm the reverse proxy is sending the `X-Forwarded-Proto` header

### HSTS header not appearing

**Symptom**: The `Strict-Transport-Security` header is not present in responses.

**Cause**: HSTS headers are only sent over HTTPS connections, or the setting is not enabled.

**Solutions**:

- Verify `EnableHsts` is set to `true`
- Confirm you are accessing via HTTPS (HSTS headers are not sent over HTTP)
- When using a reverse proxy, ensure the request is recognized as HTTPS via `X-Forwarded-Proto`

### Browser shows certificate warning

**Symptom**: Browser displays "Your connection is not private" or similar certificate warnings.

**Cause**: The certificate is self-signed, expired, or doesn't match the hostname.

**Solutions**:

- Use a certificate from a trusted Certificate Authority for production
- Ensure the certificate's Common Name (CN) or Subject Alternative Name (SAN) matches the hostname
- Check that the certificate has not expired
- For internal/development use, add the self-signed certificate to the trusted root store
