---
name: particular-docs-review
description: Reviews Particular Platform documentation for en-US language standards, style consistency, technical accuracy, and quality. Use when reviewing or editing Particular Platform documentation.
allowed-tools:
  - shell(dotnet tool restore)
  - shell(dotnet docstool:*)
  - shell(grep:*)
---

# Particular Documentation Review

Review docs for language accuracy, technical correctness, and consistency. Update `reviewed` field in frontmatter: `reviewed: 2026-06-01`

---

# Language Standards (en-US)

## Spelling

Use en-US American English. Common: -ize/yze (organize, realize, analyze), -or (color, behavior), -er (center, meter), -ence (defense, license), practice (both noun/verb). Corrections: colour→color, centre→center, organise→organize, licence→license, defence→defense, programme→program, practise→practice.

When correcting, explicitly state alignment with en-US American English.

---

## Grammar

Serial comma ("A, B, and C"). Collective nouns singular ("The team is"). Restrictive clauses use "that"; non-restrictive use "which" (with comma). Prefer active voice: "processes" not "are processed by".

---

## Dates and Time

Dates: Month Day, Year (January 15, 2025). Time: 12-hour with AM/PM. Write months fully.

---

## Numbers

Commas for thousands (1,000). Periods for decimals (3.14). Spell 0-10 as words. Numerals for 11+ and measurements (15 MB).

---

# Writing Principles

Write for experienced devs. Explain why before how. Start with overview. Define terms on first use. Concrete examples. One idea per paragraph. Sentences ~25 words max. Lists for 3+ items. Clear heading hierarchy.

## User Focus

Start with the reader's task or decision, not the implementation change.

- Keep details that help users configure, operate, or troubleshoot the feature; explain what they need to do and why
- Omit internal call sequences, caching, client lifetimes, and exception or logging mechanics unless they change user action or explain a documented symptom
- Preserve prerequisites, defaults, permissions, failure outcomes, and data-loss warnings when simplifying
- Keep behavior in one primary location and link from related pages rather than repeating the explanation

For example, explain when publishers create topics and when users must provision them; omit per-client existence-check caching.

## Versioned Product Documentation

Review from the reader's perspective for that version, not the transition that created the change.

- Present the recommended API/pattern as the way to use it for that version
- Avoid framing the recommended path as one more choice unless there are supported alternatives for new work
- Move migration details, obsolete APIs, and compatibility notes to upgrade guides when they help existing users move forward
- Use versioned partials or callout boxes for older-supported-version notes
- Each major has one view. Describe the latest minor of that major and name the version in the text for minor features ("In version 9.2 and later, ..."), including the earlier default when a minor changes one
- Content useful only while old minor is supported should not dominate newer major version pages
- Include history only when it helps current decisions
- For multi-version pages, scope guidance per version without forcing new users through legacy options
- **Configuration examples:** outdated config syntax is a critical issue even if still valid — update to current patterns

---

# Code Samples

Prefer snippets from `Snippets/{ComponentKey}/` referenced via `snippet:` key over inline blocks. Component keys in `components/component.yaml`. Inline only for very small examples.

Samples must:

- Compile or clearly state pseudo-code
- Use current APIs
- Include required imports
- Use clear variable names
- Explain non-obvious logic
- Avoid hardcoded configuration values
- Samples should define `LangVersion` to match the default language version of the lowest .NET target framework used in the solution
- Snippet projects should not define `LangVersion`
- Use exact package versions for prerelease packages; do not use wildcard prerelease versions such as `1.*-*`
- Add `prerelease.txt` to a versioned sample or snippet directory when its projects reference prerelease versions of that component's packages, and do not add it otherwise

---

# Includes

`include: KEY` references a shared markdown file. MUST find and review every include. Includes are rendered inline and shared — changes affect every page referencing them.

**To find:** search from document directory upward for `{key}.include.md`:
```bash
find . -name "{key}.include.md"
```

---

# Partials

`partial: KEY` inserts version-specific content. MUST find and review every partial. Convention: `{filePrefix}_{key}_{nugetAlias}_{version}.partial.md` (same dir as document, filePrefix = document filename without .md). Search for `{filePrefix}_{key}_*.partial.md` and review all versions. Check every partial's range against [Supported Versions](#supported-versions).

Partial and `#if-version` ranges must start at a major. A minor bound such as `[6.5,)` is a critical issue: overlapping partials (`[6,6.5)` and `[6.5,)`) both match the 6.x view and only one renders, and an `#if-version [6.5,)` block is hidden in the 6.x view. `docstool test` does not catch either case.

---

# Snippets

`snippet: KEY` references code from `Snippets/{ComponentKey}/`. MUST check all referenced snippets. Focus: verify snippet is relevant to surrounding docs and illustrates what text claims. If changed, apply full Code Review Checklist.

**To find:** note key after `snippet:`, get component from frontmatter `component:`, look up key in `components/components.yaml`, search `Snippets/{ComponentKey}/` for the marker type:

| File type | Open | Close |
|-----------|------|-------|
| C# (region) | `#region KEY` | `#endregion` |
| C# (comment) | `// startcode KEY` | `// endcode` |
| XML | `<!-- startcode KEY -->` | `<!-- endcode -->` |
| PowerShell | `# startcode KEY` | `# endcode` |
| SQL | `-- startcode KEY` | `-- endcode` |
| Plain text | `startcode KEY` | `endcode` |
| Docker / YAML | `# startcode KEY` | `# endcode` |

Specify marker type when locating snippet code.

---

# Supported Versions

Content that only applies to unsupported versions is obsolete, even if it is accurate for that version. MUST check every partial range, `#if-version` block, snippet version directory, and prose version qualifier (for example "In versions 4.3 and above") against the supported versions.

**Source:** `nservicebus/upgrades/supported-versions.json` lists the supported minors per NuGet package; `Mainstream` and `Extended` both count as supported. It is generated; never edit it. End dates are in `supported-versions-nservicebus.include.md` and `supported-versions-downstreams.include.md` in the same folder. `all-versions.include.md` also lists unsupported versions, so don't use it as the source.

**Map the page to a package** via its frontmatter `component:` in `components/nugetAlias.txt` (for example `Rabbit: NServiceBus.RabbitMQ`); partial file names use the same alias. Pages without `component:` have no version dropdown; check their version mentions against the package they name.

```bash
# Output can run into the next package; stop at this package's closing `]`
grep -A30 '"PackageId": "NServiceBus.RabbitMQ"' nservicebus/upgrades/supported-versions.json
grep -A12 '\[NServiceBus.RabbitMQ\]' nservicebus/upgrades/supported-versions-downstreams.include.md
```

Support is per minor and can skip a major. Only versions below the oldest supported major are retired; leave unsupported majors between supported ones (for example NServiceBus.RabbitMQ 9, between 8 and 10) unless asked.

**In the page under review:**

- Remove prose version qualifiers that every documented version already meets, and rewrite the sentence for the current reader
- Delete partials and `#if-version` blocks whose range only covers versions below the oldest supported major, even when a snippet directory for that version still exists. Also delete empty sibling partials for the same key, and remove the `partial:` line when no sibling with content remains
- Remove content that only helps readers migrate from, or interoperate with, a version below the oldest supported major, even when it sits in a partial whose range covers supported versions (for example the RabbitMQ v1 to v2 delay infrastructure migration). Check that the upgrade guide covers it first
- Before removing a heading, search for inbound links to its anchor (see Anchor Stability)

**Component-wide retirement:** a snippet directory for a version below the oldest supported major (for example `Snippets/Rabbit/Rabbit_6` when the oldest supported major is 7) keeps that version in the dropdown on every page for the component. Removing that version's partials and content from the page under review is part of the review. Deleting the snippet directory and cleaning up the other pages for the component is not: report it as a follow-up with the affected files, and do not do it inside a single-page review unless asked. Retiring a version (see commit `eb046ce1a6`, "Remove RabbitMQ 5.x docs") means:

- Delete its snippet and sample version directories, and remove the projects from the solution file
- Delete partials and `#if-version` blocks that only covered it, on every page of the component
- Widen or merge the remaining partials so their ranges cover all documented versions without overlapping

If a supported version's "Extended Until" date is within 30 days, report the date and ask whether to retire it now.

Upgrade guides under `nservicebus/upgrades/` and `transports/upgrades/` are exempt; they describe old versions on purpose. Report obsolete version content under Technical Updates.

---

# Terminology

One term per concept. Avoid synonyms that confuse. Use standard industry terms. Keep casing consistent. Fix every occurrence, not some.

---

# Markdown Rules

ATX headers (`## Header`). Blank line before/after headers. Use `-` for lists, numbered for sequences. Blank line before/after lists. Language identifiers in code fences. Use `code` for variables. Descriptive link text — never "click here".

---

# Content Quality Checks

Verify: API signatures correct, config syntax valid, versions current, deprecated APIs removed/marked, namespaces/packages correct.

Include what the reader needs for the page's task: feature purpose, when and how to use it, relevant configuration, and actionable pitfalls or troubleshooting. Do not turn this into an exhaustive inventory of implementation behavior.

---

# Code Review Checklist

Per sample: compiles, imports present, naming clear, current conventions, error handling where relevant, logic comments where needed, formatting consistent, no deprecated patterns.

When reviewing a code sample, explicitly reference this checklist and note verification of each item.

---

# Link Validation

Internal links resolve. External links work. API links reference correct versions. No broken URLs. Link text describes destination.

---

# Anchor Stability

Docs engine generates anchors from heading stack. New H2 resets the stack; sub-headings append. Reparenting a heading under a new H2 changes its slug and breaks external links. When restructuring, keep headings at original level or verify cross-page links.

---

# Review Output

Format: Summary, Critical Issues, Improvements, Technical Updates, Suggestions.

**Distinguish:** broken code/missing imports/deprecated API/wrong config = critical. British spelling/passive voice/missing blank line/awkward phrasing = improvement/suggestion. Do not elevate style to critical.

Example:
```markdown
### Critical Issues
- Line 45: Non‑existent method `GetMessageAsync()` → use `GetMessage()`
### Improvements
- Line 23: "colour" → "color"
### Technical Updates
- Line 100: Obsolete `.ConfigureAwait(false)`
```

---

# Review Focus

**New docs:** technical correctness, completeness, clarity, working code samples.
**Updated docs:** what changed, previous issues fixed, new problems, consistency with surrounding content.
**Legacy docs:** technical currency, modernization opportunities, obsolete content removal (see [Supported Versions](#supported-versions)), deprecated features.

---

# Conservative Change Philosophy

Only suggest changes backed by rules in this file. If text is correct and understandable, leave it. Prefer consistency with existing docs. Avoid churn. Removing content that only applies to unsupported versions is not churn; do not downgrade it to a suggestion because the text is accurate for an old version.

---

# Local Validation

Validate with the docstool version pinned in the repository's `.config/dotnet-tools.json`. Run from the repository root before submitting:
```bash
dotnet tool restore
dotnet docstool test --no-version-check
```

`dotnet tool restore` only restores the tools the repository declares; it does not install anything globally. Do not install docstool with `dotnet tool install -g` or `dotnet tool update -g`.

If `dotnet` is missing, the restore fails, or the Particular feed (`f.feedz.io`) is unreachable, report validation as blocked and recommend rerunning the task on GitHub Actions. Do not work around it.

If stale generated files cause failures, ask before running `git clean -dxf`; it deletes all untracked files.

`dotnet docstool test` passing is necessary but not sufficient — it does not validate external links or content quality. Link validation and content review must be done manually.
