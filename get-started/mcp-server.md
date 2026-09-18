---
title: Particular Docs MCP Server overview
summary: The Particular Docs MCP Server is a Model Context Protocol server that adds up-to-date documentation as context to language models
reviewed: 2028-09-18
suppressRelated: false
---

The Particular Software Model Context Protocol (MCP) Server enables clients like GitHub Copilot, Claude Code, and other AI agents to bring up-to-date information directly from the official NServiceBus and Particular Service Platform documentation. It's a remote MCP server using streamable http. It allows agents to search through documentation and fetch complete articles.

## Use cases

- Enhance agentic development environments like Visual Studio, JetBrains Rider, and Visual Studio Code with documentation content
- Use official, up-to-date documentation content in Copilot agents, Claude Code agents, and custom solutions, not stale data from model training
- Enable learners, engineers, and support to use official documentation content in their flow of work

## How the MCP Server works

The Particular Docs MCP Server is a remote [MCP](https://modelcontextprotocol.io/) server that uses streamable http. Compatible client apps can connect with the endpoint.

```html
https://docs.particular.net/mcp
```

> [!NOTE]
> This endpoint is designed for programmatic access by MCP clients via Streamable HTTP. Direct requests from a web browser will return `405 Method Not Allowed`.

## Requirements

There is no authentication required to access the MCP server. Users can use their preferred MCP client or agentic development environment.
