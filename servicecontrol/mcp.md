---
title: Model Context Protocol (MCP) integration
summary: Integrate AI tools with ServiceControl using the Model Context Protocol.
reviewed: 2026-03-21
component: ServiceControl
related:
- servicecontrol/servicecontrol-instances/configuration
- servicecontrol/audit-instances/configuration
---

ServiceControl can expose a [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) server, allowing AI-powered tools and assistants to interact with failed and audit message data directly.

When enabled, the MCP server is available at the `/mcp` path on the ServiceControl instance's HTTP endpoint. The server uses the HTTP Streamable transport and supports standard MCP tool discovery and invocation.

## Enabling MCP

The MCP server is disabled by default and must be enabled separately on each instance type.

### Error instance

See [EnableMcpServer](/servicecontrol/servicecontrol-instances/configuration.md#enablemcpserver) in the error instance configuration settings.

### Audit instance

See [EnableMcpServer](/servicecontrol/audit-instances/configuration.md#enablemcpserver) in the audit instance configuration settings.

## Connecting an MCP client

Once enabled, configure an MCP client to connect to the ServiceControl instance using the HTTP Streamable transport. For example, an error instance running on its default address would be accessible at:

```
http://localhost:33333/mcp
```

An audit instance running on its default address would be accessible at:

```
http://localhost:44444/mcp
```

The exact host and port depend on the [host settings](/servicecontrol/setting-custom-hostname.md) of each instance.

> [!NOTE]
> When [authentication](/servicecontrol/security/configuration/authentication.md) is enabled, MCP clients must provide appropriate credentials.

### Client configuration examples

Most MCP clients accept a server configuration in JSON format. The following example shows how to configure a connection to a ServiceControl error instance:

```json
{
  "mcpServers": {
    "servicecontrol": {
      "url": "http://localhost:33333/mcp"
    },
    "servicecontrol-audit": {
      "url": "http://localhost:44444/mcp"
    }
  }
}
```

## Available tools

The MCP server exposes tools for querying and managing messages. The tools available depend on the instance type.

### Error instance tools

#### Failed messages

| Tool | Description |
| --- | --- |
| `GetFailedMessages` | Get failed messages with filtering by status, modified date, and queue address |
| `GetFailedMessageById` | Get details of a specific failed message |
| `GetFailedMessageLastAttempt` | Get the last processing attempt for a failed message |
| `GetErrorsSummary` | Get error counts grouped by status |
| `GetFailedMessagesByEndpoint` | Get failed messages for a specific endpoint |

#### Retries

| Tool | Description |
| --- | --- |
| `RetryFailedMessage` | Retry a single failed message by sending it back to its original queue |
| `RetryFailedMessages` | Retry multiple failed messages by their IDs |
| `RetryFailedMessagesByQueue` | Retry all failed messages from a specific queue address |
| `RetryAllFailedMessages` | Retry all failed messages across all queues |
| `RetryAllFailedMessagesByEndpoint` | Retry all failed messages for a specific endpoint |
| `RetryFailureGroup` | Retry all failed messages in a failure group |

#### Archiving

| Tool | Description |
| --- | --- |
| `ArchiveFailedMessage` | Archive a single failed message |
| `ArchiveFailedMessages` | Archive multiple failed messages by their IDs |
| `ArchiveFailureGroup` | Archive all failed messages in a failure group |
| `UnarchiveFailedMessage` | Unarchive a single failed message back to unresolved |
| `UnarchiveFailedMessages` | Unarchive multiple failed messages by their IDs |
| `UnarchiveFailureGroup` | Unarchive all failed messages in a failure group |

#### Failure groups

| Tool | Description |
| --- | --- |
| `GetFailureGroups` | Get failure groups (messages grouped by exception type and stack trace) |
| `GetRetryHistory` | Get history of past retry operations and their outcomes |

### Audit instance tools

#### Audit messages

| Tool | Description |
| --- | --- |
| `GetAuditMessages` | Get successfully processed audit messages with paging and sorting |
| `SearchAuditMessages` | Search audit messages by keyword across content and metadata |
| `GetAuditMessagesByEndpoint` | Get audit messages received by a specific endpoint |
| `GetAuditMessagesByConversation` | Get all audit messages in a conversation |
| `GetAuditMessageBody` | Get the body content of a specific audit message |

#### Endpoints

| Tool | Description |
| --- | --- |
| `GetKnownEndpoints` | List all known endpoints that have sent or received audit messages |
| `GetEndpointAuditCounts` | Get audit message counts per day for a specific endpoint |
