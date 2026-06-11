---
name: particular-docs-review
description: Reviews Particular Platform documentation for en-US language standards, style consistency, technical accuracy, and quality. Use when reviewing or editing Particular Platform documentation.
invocable: false
---

# Particular Documentation Review

Review docs for language accuracy, technical correctness, and consistency. Update `reviewed` field in frontmatter: `reviewed: 2026-06-01`

---

# Language Standards (en-US)

## Spelling

Use en-US American English. Common: -ize (organize, realize, analyze), -or (color, behavior), -er (center, meter), -ence (defense, license), practice (both noun/verb). Corrections: colour→color, centre→center, organise→organize, licence→license, defence→defense, programme→program, practise→practice.

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

## Versioned Product Documentation

Review from the reader's perspective for that version, not the transition that created the change.

- Present the recommended API/pattern as the way to use it for that version
- Avoid framing the recommended path as one more choice unless there are supported alternatives for new work
- Move migration details, obsolete APIs, and compatibility notes to upgrade guides when they help existing users move forward
- Use versioned partials or callout boxes for older-supported-version notes
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

`partial: KEY` inserts version-specific content. MUST find and review every partial. Convention: `{filePrefix}_{key}_{nugetAlias}_{version}.partial.md` (same dir as document, filePrefix = document filename without .md). Search for `{filePrefix}_{key}_*.partial.md` and review all versions.

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

# Terminology

One term per concept. Avoid synonyms that confuse. Use standard industry terms. Keep casing consistent. Fix every occurrence, not some.

---

# Markdown Rules

ATX headers (`## Header`). Blank line before/after headers. Use `-` for lists, numbered for sequences. Blank line before/after lists. Language identifiers in code fences. Use `code` for variables. Descriptive link text — never "click here".

---

# Content Quality Checks

Verify: API signatures correct, config syntax valid, versions current, deprecated APIs removed/marked, namespaces/packages correct.

Docs must include: what feature does, when to use, how to use, config options, pitfalls/edge cases, troubleshooting.

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
**Legacy docs:** technical currency, modernization opportunities, obsolete content removal, deprecated features.

---

# Conservative Change Philosophy

Only suggest changes backed by rules in this file. If text is correct and understandable, leave it. Prefer consistency with existing docs. Avoid churn.

---

# Local Validation

Run `docstool test --no-version-check` before submitting.
```bash
dotnet tool install -g Particular.DocsTool --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
dotnet tool update -g Particular.DocsTool --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
docstool test --no-version-check
git clean -dxf  # if stale files
```

`docstool test` passing is necessary but not sufficient — it does not validate external links or content quality. Link validation and content review must be done manually.
```
