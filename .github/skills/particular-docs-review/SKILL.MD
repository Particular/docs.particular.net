---
name: particular-docs-review
description: Reviews Particular Platform documentation for en-US language standards, style consistency, technical accuracy, and quality. Use when reviewing or editing Particular Platform documentation.
invocable: false
---

# Particular Documentation Review

Review Particular Platform documentation for language accuracy, technical correctness, and consistency.

When reviewing a file, update the `reviewed` field in frontmatter using ISO format:

```yaml
reviewed: 2026-02-23
```

---

# Language Standards (en-US)

## Spelling

Use American English spelling.

Examples:
- organize, realize, analyze
- color, flavor, behavior
- center, meter, theater
- defense, offense, license
- practice (noun and verb)

Common corrections:

- colour -> color
- centre -> center
- organise -> organize
- licence -> license
- defence -> defense
- programme -> program
- practise -> practice

---

## Grammar

- Use serial comma: "A, B, and C"
- Collective nouns are singular: "The team is"
- Restrictive clauses use "that"
- Non‑restrictive clauses use "which"
- Prefer active voice

---

## Dates and Time

- Dates: Month Day, Year (January 15, 2025)
- Time: 12-hour with AM/PM
- Avoid 24-hour format in user docs
- Write months fully

---

## Numbers

- Use commas for thousands: 1,000
- Use periods for decimals: 3.14
- Spell out numbers 0-10
- Use numerals for 11+ and measurements

---

# Writing Principles

Write for experienced software developers.

- Explain why before how
- Start with a high‑level overview
- Define terms on first use
- Use consistent terminology
- Provide concrete examples
- One idea per paragraph
- Prefer sentences under ~25 words
- Use lists for 3+ items
- Use clear heading hierarchy

---

# Code Samples

Prefer snippet projects over inline code blocks.

Rules:

- Use snippets from `snippets/{ComponentKey}/`
- Reference them with the `snippet:` key
- Component keys come from `components/component.yaml`
- Inline fenced blocks only for very small examples

Code samples should:

- Compile or clearly state pseudo‑code
- Use current APIs
- Include required imports
- Use clear variable names
- Explain non‑obvious logic
- Avoid hardcoded configuration values

---

# Terminology

Use consistent terminology.

Rules:

- One term per concept
- Avoid synonyms that cause ambiguity
- Use standard industry terminology
- Keep casing consistent

Example:

Correct: message handler  
Incorrect: handler / messageprocessor

---

# Markdown Rules

- Use ATX headers (`## Header`)
- Leave a blank line before and after headers
- Use `-` for lists
- Use numbered lists for sequences
- Leave a blank line before and after lists
- Include language identifiers in code fences
- Use `code` formatting for variables
- Use descriptive link text

Example:

Correct: `[Message handler configuration](/nservicebus/messaging/handlers.md)`  
Incorrect: `[click here](/page)`

---

# Content Quality Checks

Verify:

- API signatures are correct
- Configuration syntax is valid
- Versions are current
- Deprecated APIs are removed or marked
- Namespaces and packages are correct

Documentation should include:

- What the feature does
- When to use it
- How to use it
- Configuration options
- Pitfalls or edge cases
- Troubleshooting guidance

---

# Code Review Checklist

Check each sample:

- Code compiles
- Imports are included
- Naming is clear
- Current conventions are used
- Error handling appears where relevant
- Logic comments exist where needed
- Formatting is consistent
- No deprecated patterns

---

# Link Validation

Ensure:

- Internal links resolve
- External links work
- API links reference correct versions
- No broken URLs
- Link text describes destination

---

# Anchor Stability

The docs engine generates anchors from a heading stack. A new H2 resets the stack; sub-headings append to it. `## Heading 1` followed by `### Heading 1.1` produces `#heading-1-heading-1-1`. Reparenting a heading under a new H2 changes its slug and breaks external links.

When restructuring pages, keep headings at their original level or verify cross-page links. Prefer versioned partials over wrapping legacy sections under new H2s.

---

# Common Issues

## Spelling

- British spelling variants
- Grammar mistakes
- Homophone confusion
- Run-on sentences

## Style

- Mixed terminology
- Inconsistent headers
- Curly quotes instead of straight quotes
- Mixed list styles

## Technical

- Outdated APIs
- Deprecated patterns
- Missing imports
- Incorrect configuration
- Version mismatches

---

# Review Output

Use this format:

1. Summary
2. Critical Issues
3. Improvements
4. Technical Updates
5. Suggestions

Example:

```markdown
## Review of: /path/to/file.md

### Critical Issues
- Line 45: Non‑existent method `GetMessageAsync()`
  Suggestion: Use `GetMessage()`

### Improvements
- Line 23: "colour" -> "color"

### Technical Updates
- Line 100: Obsolete `.ConfigureAwait(false)`
```

---

# Review Focus

## New Docs

Prioritize:

- Technical correctness
- Completeness
- Clarity
- Working code samples

## Updated Docs

Check:

- What changed
- Issues fixed
- New problems introduced
- Consistency with surrounding docs

## Legacy Docs

Evaluate:

- Technical currency
- Opportunities to modernize
- Removal of obsolete content
- Deprecated features

---

# Conservative Change Philosophy

Avoid unnecessary edits.

Rules:

- Only suggest changes backed by rules in this file
- If text is correct and understandable, leave it
- Prefer consistency with existing docs
- Avoid churn in diffs and reviews

---

# Local Validation

Run the docs tool before submitting changes.

In Copilot cloud agent sessions for this repository, `docstool` is preinstalled by `.github/workflows/copilot-setup-steps.yml`.

Install:

```bash
dotnet tool install -g Particular.DocsTool --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
```

Update:

```bash
dotnet tool update -g Particular.DocsTool --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
```

Run:

```bash
docstool test --no-version-check
```

If you encounter stale files:

```bash
git clean -dxf
```
