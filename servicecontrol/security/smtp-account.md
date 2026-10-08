---
title: Securing the SMTP account for email notifications
summary: How ServiceControl stores the SMTP password for email notifications, and how to limit the impact if it is disclosed
reviewed: 2026-10-09
component: ServiceControl
related:
- servicepulse/health-check-notifications
---

ServiceControl can send [health check notifications](/servicepulse/health-check-notifications.md) by email. To authenticate with the SMTP server, ServiceControl stores the SMTP account and password in its database.

## How the password is handled

- The password is stored in the ServiceControl database. Everyone who can read that database can read the password.
- From ServiceControl version 6.21.2, the API never returns the stored password. In ServicePulse, the password field is empty, even when a password is stored.
- Leave the password field empty to keep the stored password, or enter a new password to replace it.
- To remove the stored password, clear the authentication account.

## Securing the SMTP account

To limit the damage if the password is disclosed:

- Use a dedicated SMTP account for ServiceControl notifications. Do not use an account that people or other applications also use.
- Limit what the account can send: only from the configured From address, and only to the configured To addresses.
- Limit access to the ServiceControl database:
  - With the RavenDB persister in a container, do not expose the RavenDB port outside the container network.
  - With the SQL Server or PostgreSQL persister, grant database access only to the ServiceControl instance and to administrators.
- Change the password of the SMTP account if it might have been disclosed.
