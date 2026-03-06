# Agent Instructions

When reviewing or editing documentation in this repository, use the `particular-docs-review` skill.

Skill definition:
- `.github/skills/particular-docs-review/SKILL.MD`

Primary guidance for that skill includes:
- en-US language and style consistency
- technical correctness for samples and APIs
- conservative edits that avoid unnecessary churn
- updating frontmatter `reviewed` date when performing review passes

This file is the canonical agent instruction entry point for tools that support `AGENTS.md` discovery.

## Repository Organization

- Product documentation pages are organized by topic folders at repository root (for example `nservicebus/`, `transports/`, `tutorials/`, `servicecontrol/`, and `platform/`).
- Navigation structure is defined in `menu/`.
- Reusable documentation building blocks are in `components/`.
- Code assets are in `samples/` and `Snippets/`.
- `samples/` and `Snippets/` are versioned content. Preserve version-specific structure and update the correct version target rather than flattening or merging versions.
