### HTTPS redirect loops

**Symptom**: Browser shows "too many redirects" or redirect loop errors when accessing the application through a reverse proxy with SSL termination.

**Cause**: The `X-Forwarded-Proto` header is not being processed, so the application thinks the request is HTTP and keeps redirecting to HTTPS.

**Solutions**:

- Verify forwarded headers are enabled
- Check that the proxy IP is in `KnownProxies` or `KnownNetworks`, or enable `TrustAllProxies`
- Confirm the reverse proxy is sending the `X-Forwarded-Proto` header

### Wrong host in generated URLs

**Symptom**: Links or redirects use the internal hostname (e.g. `localhost` or container name) instead of the public hostname.

**Cause**: The `X-Forwarded-Host` header is not being trusted or processed.

**Solutions**:

- Verify the proxy IP is trusted (check `KnownProxies`/`KnownNetworks` configuration)
- Confirm the reverse proxy is sending the `X-Forwarded-Host` header
- In Kubernetes, ensure the correct pod/node network is in `KnownNetworks`

### Client IP shows proxy IP instead of real client

**Symptom**: Logs or audit trails show the proxy's IP address instead of the actual client IP.

**Cause**: The `X-Forwarded-For` header is not being processed, or the wrong IP is being extracted from a proxy chain.

**Solutions**:

- Verify forwarded headers are enabled and the proxy is trusted
- For proxy chains, check the `ForwardLimit` behavior — with `TrustAllProxies=false`, only the last proxy IP is used
- If using multiple proxies, ensure all proxy IPs are listed in `KnownProxies`

### Headers not processed in Kubernetes

**Symptom**: Forward headers work locally but not in Kubernetes, even with headers enabled.

**Cause**: In Kubernetes, the ingress controller or load balancer IP may differ from what's configured in `KnownProxies`.

**Solutions**:

- Use `KnownNetworks` with the pod/service CIDR range instead of specific IPs (e.g. `10.0.0.0/8`)
- Check what IP the request is actually coming from (ingress controller pod IP, node IP, or load balancer IP)
- For development/testing, temporarily enable `TrustAllProxies=true` to confirm headers are the issue

### Verifying header processing

To confirm whether forwarded headers are being processed correctly:

1. Enable verbose logging to see incoming request details
2. Check that `Request.Scheme` shows `https` (not `http`) when accessing via HTTPS-terminating proxy
3. Verify the `Host` header in logs matches the expected public hostname
4. Compare client IP in logs against the expected client IP
