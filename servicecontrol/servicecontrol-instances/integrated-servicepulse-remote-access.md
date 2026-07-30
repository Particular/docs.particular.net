---
title: Remote access to integrated ServicePulse
summary: How to make integrated ServicePulse reachable from other machines without installing IIS or a standalone ServicePulse
reviewed: 2026-07-30
component: ServiceControl
related:
- servicecontrol/servicecontrol-instances/integrated-servicepulse
- servicecontrol/security
- servicecontrol/setting-custom-hostname
- servicepulse/install-servicepulse-in-iis
---

By default a ServiceControl Error instance only accepts connections from the machine it runs on, so [integrated ServicePulse](integrated-servicepulse.md) can only be opened in a browser on that machine. This guide describes how to make it available to other people on the network.

> [!NOTE]
> The instructions for [hosting ServicePulse in IIS](/servicepulse/install-servicepulse-in-iis.md) do **not** apply to integrated ServicePulse. Neither IIS nor a standalone ServicePulse installation is required.

## Why integrated ServicePulse needs less configuration

A standalone ServicePulse is a separate web application with its own address, and it has to be told where to find the ServiceControl API. Because the two sit at different addresses, they need URL rewriting or extra browser permissions to work together. This is why the IIS instructions are as long as they are.

Integrated ServicePulse is served by the ServiceControl Error instance itself, at the same address that serves the API. It does not store an address for the API at all; it simply asks for `/api` on whatever address the browser used to reach it:

| Address                       | Serves                                |
|-------------------------------|---------------------------------------|
| `http://<host>:33333/`        | Integrated ServicePulse               |
| `http://<host>:33333/api`     | ServiceControl Error instance API     |

Because ServicePulse and the API always share an address, making ServicePulse available to other machines needs no URL rewriting, no [CORS](/servicecontrol/security/configuration/cors.md) configuration, and no edits to `app.constants.js`. Changing the host name is enough.

## Step 1: Accept connections from other machines

> [!WARNING]
> Anyone who can reach this address has full access to the message data stored in ServiceControl, including message bodies and headers, and can retry, edit, archive, and delete messages. Do not expose an instance without also completing [Step 3: Secure the deployment](#step-3-secure-the-deployment).

Change [`ServiceControl/HostName`](configuration.md#host-settings-servicecontrolhostname) from `localhost` to either a specific host name (for example `sc.example.com`) or `*` to accept connections addressed to any host name.

Using ServiceControl Management:

1. Click the Configuration icon for the Error instance.
1. Change the Host Name field.
1. Click Save. ServiceControl Management validates the change and restarts the service.

Or edit `ServiceControl.exe.config` directly and restart the service:

```xml
<appSettings>
  <add key="ServiceControl/HostName" value="*" />
</appSettings>
```

> [!WARNING]
> Before changing the host name, confirm that `ServiceControl/DbPath` is present in `ServiceControl.exe.config`. When that setting is absent, the default database folder name is derived from the host name and port, so changing either one points ServiceControl at a different, empty database. Instances deployed with ServiceControl Management or PowerShell have `ServiceControl/DbPath` written explicitly and are unaffected. See [RavenDB embedded location](/servicecontrol/configure-ravendb-location.md).

Containers already accept connections addressed to any host name and need no change; expose the container's port `33333` instead.

### Choosing a port

Now that users type this address themselves, the default port `33333` is worth reconsidering. It can be changed with [`ServiceControl/Port`](configuration.md#host-settings-servicecontrolport).

The recommended port is `443`, the standard port for HTTPS. Browsers hide it, so users get a clean address with no port number.

```xml
<appSettings>
  <add key="ServiceControl/HostName" value="servicepulse.example.com" />
  <add key="ServiceControl/Port" value="443" />
  <add key="ServiceControl/Https.Enabled" value="true" />
</appSettings>
```

This gives `https://servicepulse.example.com` and combines with the TLS certificate from [Step 3](#step-3-secure-the-deployment).

Before changing the port, check that:

- nothing else on the machine already uses it, IIS in particular, which usually claims `443`
- the [`ServiceControl/DbPath`](configuration.md#embedded-database-servicecontroldbpath) warning above is satisfied, because the default database folder name includes the port as well as the host name
- any other tools or scripts calling the API directly are updated, along with the [`ServiceControl/RemoteInstances`](configuration.md#host-settings-servicecontrolremoteinstances) configuration of any *other* Error instance configured to read from this one, which only applies to federated setups

Alternatively, leave ServiceControl's port alone and put a [reverse proxy](#using-a-reverse-proxy-instead) in front to publish it on `443`.

## Step 2: Allow the port through the firewall

Add an inbound firewall rule on the ServiceControl machine for the Error instance port (`33333` by default). On Windows this can be done through the Windows Defender Firewall console or with PowerShell:

```powershell
New-NetFirewallRule -DisplayName "ServiceControl (integrated ServicePulse)" -Direction Inbound -Protocol TCP -LocalPort 33333 -Action Allow
```

Microsoft documents both approaches in [Windows Firewall rules](https://learn.microsoft.com/en-us/windows/security/operating-system-security/network-security/windows-firewall/rules) and the [`New-NetFirewallRule` reference](https://learn.microsoft.com/en-us/powershell/module/netsecurity/new-netfirewallrule).

Network firewalls, proxies, or DNS between users and the machine may also need changes. If another team is responsible for the network or security infrastructure, involve them at this point. They should configure the network to allow inbound TCP traffic to this port on this machine from the users who require access.

> [!NOTE]
> No `netsh http add urlacl` command is needed. That step appears in the ServicePulse in IIS instructions because it applies to standalone ServicePulse, which uses a different web server. ServiceControl does not require it.

Users can now open `http://<host>:33333/`.

## Step 3: Secure the deployment

Once the instance accepts connections from other machines, the protection that came from only accepting local connections is gone, and access has to be controlled explicitly.

### Encrypt traffic with TLS

Without TLS, message data and sign-in credentials cross the network in plain text, readable by anyone able to capture that traffic. This applies on an internal network too. See [TLS configuration](/servicecontrol/security/configuration/tls.md) for the settings; integrated ServicePulse uses the TLS configuration of the Error instance that hosts it, so one certificate covers both.

A certificate is required, in PFX (PKCS#12) format. Where to get one depends on the name users will type:

- **An internal-only name**, such as `cm.corp.local` or a name that resolves only inside the network. Use the organization's own certificate authority, for example Active Directory Certificate Services. Certificates it issues are already trusted by domain-joined machines, so browsers show no warning. Public certificate authorities cannot issue certificates for names they are unable to verify.
- **A name under a public domain the organization owns**, such as `servicepulse.example.com`, even if it only resolves internally. A public certificate authority can issue a certificate, including free ones from [Let's Encrypt](https://letsencrypt.org/). Use DNS-based validation (DNS-01), which proves ownership by adding a DNS record and does not require the server to be reachable from the internet. Note that these certificates are short-lived, so set up automatic renewal.
- **Self-signed certificates** are not recommended here. Every user gets a browser security warning, and training a team to click through those warnings undermines the protection TLS provides. They are reasonable only for a quick test.

### Require users to sign in

Enable [authentication](/servicecontrol/security/) so users must sign in before they can see anything. Authentication is configured on ServiceControl only; integrated ServicePulse picks up the settings from the instance that hosts it and handles the sign-in automatically.

This requires an identity provider that supports OpenID Connect. It does **not** have to be a cloud service:

- Any OpenID Connect provider already in use will work, including on-premises ones. [Keycloak](https://www.keycloak.org/) is a common self-hosted, freely available choice, and the [authentication reference](/servicecontrol/security/configuration/authentication.md#identity-provider-setup-configuration-examples-keycloak) includes a configuration example for it. Other on-premises providers that implement OpenID Connect, such as Active Directory Federation Services, are configured with the same settings.
- If no identity provider is available and standing one up is not realistic, use one of the alternatives in [If signing in is not an option](#step-3-secure-the-deployment-if-signing-in-is-not-an-option) below rather than exposing the instance unprotected.

### Limit what each user can do

With authentication alone, every signed-in user has full access. To give most of the team read-only access while a smaller group can retry or edit messages, enable [role-based access control](/servicecontrol/security/configuration/authorization.md). This requires ServiceControl 6.18.0 or later.

> [!NOTE]
> Role-based access control replaces the `SPReaders`, `SPFailedMessages`, and `SPMonitoring` roles used by the IIS URL Authorization approach in the [ServicePulse in IIS](/servicepulse/install-servicepulse-in-iis.md#advanced-configuration-role-based-security) instructions. The built-in roles are `reader`, `writer`, and `admin`.

### If signing in is not an option

Two alternatives, in order of preference:

1. **Authenticate at a reverse proxy.** Put IIS or another web server in front and let it authenticate users, for example with Windows Integrated Authentication. This is the closest equivalent to the older ServicePulse in IIS setup. See [Using a reverse proxy instead](#using-a-reverse-proxy-instead), and note the warning there about leaving ServiceControl reachable only by the proxy.
1. **Restrict access at the network layer.** Limit who can reach the port using a VPN or firewall rules scoped to specific subnets or addresses. This controls who can connect but not what they can do once connected: everyone who gets through has full access. See [Securing ServiceControl](/servicecontrol/securing-servicecontrol.md).

## Step 4: Make ServiceControl Monitoring reachable

> [!NOTE]
> Skip this step if [monitoring](/monitoring/) is not in use. To hide the Monitoring tab, set the monitoring connection to `!` as described in [Configuring the monitoring URL](#step-4-make-servicecontrol-monitoring-reachable-configuring-the-monitoring-url).

Monitoring data comes from a separate [ServiceControl Monitoring instance](/servicecontrol/monitoring-instances/) with its own host name and port. Unlike the Error instance API, it does not share an address with ServicePulse, so the browser has to be told where to find it.

The default is `http://localhost:33633/`, and in a browser on someone else's machine `localhost` means *their* machine, not the ServiceControl machine. The Monitoring tab therefore fails to load until the following are configured.

1. Set [`Monitoring/HttpHostname`](/servicecontrol/monitoring-instances/configuration.md#host-settings-monitoringhttphostname) on the Monitoring instance so it accepts connections from other machines, and allow its port (`33633` by default) through the firewall as in [Step 2](#step-2-allow-the-port-through-the-firewall).
1. Allow the ServicePulse address on the Monitoring instance with [`Monitoring/Cors.AllowedOrigins`](/servicecontrol/monitoring-instances/configuration.md#cors-monitoringcors-allowedorigins). Because the Monitoring instance is at a different address from ServicePulse, browsers require it to opt in to these requests, even though this is not needed for the Error instance API:

    ```xml
    <appSettings>
      <add key="Monitoring/HttpHostname" value="*" />
      <add key="Monitoring/Cors.AllowAnyOrigin" value="false" />
      <add key="Monitoring/Cors.AllowedOrigins" value="https://sc.example.com:33333" />
    </appSettings>
    ```

1. If [authentication](/servicecontrol/security/) is enabled, configure it on the Monitoring instance too, using the same authority and audience as the Error instance.
1. Tell ServicePulse the correct monitoring address, using one of the options below.

### Configuring the monitoring URL

- **A shared link.** Send users a link with the monitoring address included, for example `https://sc.example.com:33333/?mu=https://monitoring.example.com:33633/`. Opening it once is enough; the browser remembers the setting afterward.
- **Per user, in the ServicePulse UI.** Each user sets the monitoring address once under Configuration > Connections. See [configuring connections](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui). The ServiceControl connection cannot be edited here, because integrated ServicePulse is always connected to the instance hosting it.
- **For everyone, on the server.** Set a `MONITORING_URL` environment variable for the ServiceControl Error instance and restart the service, so every user gets the correct address without configuring anything:

    ```shell
    setx MONITORING_URL "https://monitoring.example.com:33633/" /M
    ```

    Unlike other Error instance settings, this is a plain environment variable with no `SERVICECONTROL_` prefix and cannot be set in `ServiceControl.exe.config`. Users who have already saved a monitoring address in their browser keep the one they saved.

To hide the Monitoring tab, set the monitoring address to `!`.

## Using a reverse proxy instead

A reverse proxy is a web server that sits in front of ServiceControl and forwards requests to it. Adding one is optional. It is worth doing to present ServicePulse on a friendly address such as `https://servicepulse.example.com`, to manage TLS certificates centrally alongside other internal sites, or to apply an authentication method ServiceControl does not implement itself, such as Windows Integrated Authentication.

> [!NOTE]
> ServiceControl cannot run *inside* IIS. IIS can only sit in front of it and forward requests. See the [ServiceControl hosting guide](/servicecontrol/security/hosting-guide.md#hosting-model).

Forwarding integrated ServicePulse is simpler than forwarding a standalone ServicePulse: one rule for the site root covers both the UI and the API, because they share an address. The separate `^api/(.*)` rewrite rule needed when ServicePulse and ServiceControl are separate applications does not apply.

> [!WARNING]
> When relying on the proxy to authenticate users, make sure ServiceControl itself cannot be reached directly, or users can bypass the proxy and the authentication with it. If the proxy runs on the same machine as ServiceControl, the simplest way is to leave `ServiceControl/HostName` set to `localhost` and skip [Step 1](#step-1-accept-connections-from-other-machines) entirely, so only the proxy can connect. If the proxy runs elsewhere, restrict the port to the proxy's address.

When using a reverse proxy, configure [forwarded headers](/servicecontrol/security/configuration/forward-headers.md) on the Error instance so it knows the address users actually typed. Integrated ServicePulse uses the Error instance's forwarded header configuration; there is no separate ServicePulse setting. For worked examples, see the [ServiceControl hosting guide](/servicecontrol/security/hosting-guide.md).

## Troubleshooting

### ServicePulse does not load from another machine

The Error instance is still only accepting local connections, or the port is blocked. Check the value of `ServiceControl/HostName`, that the service was restarted after the change, and that an inbound firewall rule exists. Opening `http://<host>:33333/api` from the other machine shows whether the problem is the network or ServicePulse.

### ServicePulse loads but shows no data

The browser reached ServicePulse but not the API. Check the browser developer tools Network tab (F12) for failing requests to `/api`. A leftover setting from a previous standalone ServicePulse is not the cause: integrated ServicePulse always uses the instance hosting it.

### The Monitoring tab shows "unable to connect"

The monitoring address still points at `localhost`, the Monitoring instance is not accepting connections from other machines, or the browser is blocking the request. Work through [Step 4](#step-4-make-servicecontrol-monitoring-reachable) and check the developer tools Console for CORS errors. See also [CORS troubleshooting](/servicecontrol/security/configuration/cors.md#troubleshooting).

### ServiceControl starts with an empty database after changing the host name

`ServiceControl/DbPath` was not set explicitly, so the default database location changed along with the host name. Stop the service, add `ServiceControl/DbPath` pointing at the original database folder, and restart. See [RavenDB embedded location](/servicecontrol/configure-ravendb-location.md).
