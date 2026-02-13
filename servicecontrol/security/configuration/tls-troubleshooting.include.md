### Certificate not found or access denied

**Symptom**: The application fails to start with certificate-related errors when using a PFX certificate file.

**Cause**: The certificate file path is incorrect, the file is not readable, or the password is wrong.

**Solutions**:

- Verify the certificate path is correct and the file exists
- Check that the certificate file has appropriate permissions
- Confirm the certificate password is correct
- For containers, ensure the volume mount is correct (e.g. `-v certificate.pfx:/usr/share/ParticularSoftware/certificate.pfx`)

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

**Cause**: The client (browser or another container) doesn't trust the CA that signed the server's certificate.

**Solutions**:

- Use a certificate from a trusted Certificate Authority for production
- Ensure the certificate's Common Name (CN) or Subject Alternative Name (SAN) matches the hostname
- Check that the certificate has not expired
- Ensure the certificate is in the correct Trusted Root Certificate Authorities store. e.g.
  - If running in a Windows Service as `Local System`, the certificate should be in the `Local Computer` store.
  - If running as yourself, the certificate should be in the `Current User` store.
- For internal/development use, add the self-signed certificate to the trusted root store
- For containers: Add the CA certificate to the CA bundle and set `SSL_CERT_FILE`

### The remote certificate is invalid according to the validation procedure

**Symptom**: Service starts but fails when calling other services (e.g. ServiceControl can't reach ServiceControl-Audit) or when validating tokens with Azure AD.

**Cause**: The CA bundle doesn't contain the CA certificate that signed the remote server's certificate.

**Solutions**:

- Ensure the CA bundle includes your local CA certificate (e.g. mkcert's rootCA.pem)
- For Azure AD authentication, append the Mozilla CA bundle: curl https://curl.se/ca/cacert.pem >> ca-bundle.crt
- Verify `SSL_CERT_FILE` environment variable points to the correct path inside the container
- Check the CA bundle is mounted correctly

### The certificate doesn't match the hostname

**Symptom**: Browser or client rejects the certificate with a hostname mismatch error

**Cause**: The certificate's Subject Alternative Names (SANs) don't include the hostname being used to connect.

**Solutions**:

- Regenerate the certificate with all required hostnames
- Include both the Docker service name (for container-to-container) and localhost (for host access)

### Health checks fail with certificate errors

**Symptom**: Container health check reports unhealthy; logs show SSL errors from the health check command.

**Cause**: The health check binary inside the container doesn't trust the certificate.

**Solutions**:

- Ensure `SSL_CERT_FILE` is set so the health check process can find the CA bundle
- Check the health check URL uses the correct protocol: `https://` not `http://`
- Verify the certificate includes localhost in its SANs (health checks run inside the container)