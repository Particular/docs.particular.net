---
name: particular-docs-review
description: Review Particular Platform documentation for en-US language standards, style consistency, technical accuracy, and quality. Use when evaluating or improving documentation content including spelling, grammar, code samples, clarity, and technical currency.
invocable: false
---

# Particular Documentation Review

Review documentation to ensure quality, accuracy, and consistency using en-US language standards and technical writing best practices.

When reviewing documentation, update the `reviewed` date in the frontmatter using ISO format (YYYY-MM-DD):

```yaml
reviewed: 2026-02-23
```

This ensures consistency across the documentation and allows for easy sorting and comparison.

## Language Standards (en-US)

### Spelling

Use American English spellings:
- `-ize` endings: organize, realize, analyze, initialize, customize
- `-or` endings: color, flavor, behavior, honor, labor
- `-er` endings: center, meter, theater, caliber, fiber
- `-ence` endings: defense, offense, pretense, license (noun/verb)
- noun/verb forms: practice (both noun and verb)

Common corrections:
- colour → color
- centre → center
- organise → organize
- licence → license
- defence → defense
- programme → program
- practise → practice

### Grammar Conventions

- Use serial comma (Oxford comma): "item A, item B, and item C"
- Collective nouns take singular verbs: "The team is", "The organization has"
- Restrictive clauses use "that": "The API that handles requests"
- Non-restrictive clauses use "which" (with comma): "The API, which was added in v2.0"
- Prefer active voice: "The library processes messages" over "Messages are processed by the library"
- Avoid split infinitives when possible (e.g., "to process" not "to boldly process")

### Date and Time Formatting

- Dates: Month Day, Year (e.g., "January 15, 2025")
- Time: 12-hour with AM/PM (e.g., "2:30 PM", "9:00 AM")
- Avoid 24-hour format in user-facing docs
- Write out months fully (not Jan, Feb, Mar)

### Number Formatting

- Use commas for thousands: 1,000, 100,000, 1,000,000
- Use periods for decimals: 3.14, 0.5, 99.9
- Spell out numbers 0-10 when used as counts: "five endpoints", "two methods"
- Use numerals for 11+ and for measurements: "15 MB", "20 seconds"

---

## Style Guidelines

### Clarity and Readability

- Write directly to software professionals
- Use clear, concise language - avoid wordiness
- Define technical jargon on first use
- Provide concrete examples for abstract concepts
- Structure content with clear hierarchy (H1, H2, H3)
- Use bullet points for lists of 3+ items
- Keep sentences under 25 words when possible
- One main idea per paragraph

### Technical Writing Structure

1. **Explain "why" before "how"**: Provide context before implementation details
2. **Start with overview**: Give high-level understanding before diving into details
3. **Use consistent terminology**: Establish terms early and reuse them consistently
4. **Include troubleshooting**: Address common errors and edge cases
5. **Link appropriately**: Connect to related concepts and deeper dives

### Code Sample Requirements

**Every code sample must:**
- Compile and run (or clearly mark as pseudo-code if not)
- Use current API patterns and best practices
- Include all necessary imports/using statements
- Have comments explaining non-obvious logic
- Show expected output when relevant
- Handle error cases when applicable

**Code sample format:**
- Use syntax highlighting with proper language identifier
- Indent consistently (spaces preferred over tabs)
- Line length under 100 characters when possible
- Break long lines logically (after commas, operators)

### Terminology Consistency

Establish and maintain consistent terminology:
- Choose one term per concept and stick with it
- Document terminology decisions in glossaries when helpful
- Avoid synonyms that could confuse readers
- Use standard industry terms when available
- Use consistent casing for product names and acronyms (e.g., "IBMMQ" or "IbmMq" - pick one and use throughout)

Examples:
- ✓ "message handler" (consistent)
- ✗ "message handler" / "messageprocessor" / "handler" (inconsistent)

### Markdown Formatting

- Use ATX-style headers: `## Header` (not underline style)
- Include one blank line before headers
- Use `-` for unordered lists (not `*`)
- Use numbered lists for sequences: `1. Step one`, `2. Step two`
- Code fences with language: ````csharp`, ````json`, ````bash`
- Bold for emphasis on key terms
- Code formatting for variables and placeholders: `your-api-key`, `{endpoint}`
- Links with descriptive text: [API documentation](https://...) not [click here](...)

---

## Content Quality Checks

### Accuracy and Currency

**Verify:**
- API signatures match current version
- Configuration examples use valid syntax
- Screenshots reflect current UI
- Version references are accurate
- Deprecated APIs are noted or removed
- Code uses current language features (C# 12+, etc.)
- Package names and namespaces are correct

### Completeness

**Ensure documentation includes:**
- What the feature/component does
- When to use it (use cases)
- How to use it (implementation)
- Configuration options and defaults
- Common pitfalls and edge cases
- Troubleshooting guidance
- Related resources

### Clarity

**Evaluate for:**
- Logical organization of sections
- Clear progression from simple to complex
- Appropriate level of detail for target audience
- Explanations free of ambiguity
- Examples that illustrate concepts clearly
- Minimal assumptions about reader knowledge

---

## Code Sample Review Checklist

For each code sample, verify:

- [ ] Code compiles/syntax is valid
- [ ] All imports/using statements included
- [ ] Variable names are descriptive
- [ ] Follows current coding conventions
- [ ] Error handling is shown when relevant
- [ ] Comments explain non-obvious logic
- [ ] Expected behavior is described
- [ ] No hardcoded values that should be configuration
- [ ] Indentation and formatting consistent
- [ ] Language features are current (not deprecated patterns)

---

## Link Validation

**Check all links:**
- Internal links resolve to valid sections/pages
- External links are accessible and current
- API documentation links point to correct version
- No broken URLs or redirects to error pages
- Link text is descriptive, not generic

**Link text examples:**
- ✓ [Message handler configuration](/nservicebus/messaging/handlers.md)
- ✓ [NServiceBus documentation](https://docs.particular.net/nservicebus/)
- ✗ [click here](/page)
- ✗ [more info](/another-page)

---

## Common Issues to Fix

### Spelling/Grammar

- British spellings (colour, centre, organise)
- Inconsistent verb tenses
- Subject-verb agreement errors
- Misused homophones (its/it's, their/there, your/you're)
- Run-on sentences and fragments

### Style Inconsistencies

- Mixed terminology for same concept
- Inconsistent heading levels
- Varying code sample formatting
- Inconsistent quote style (use straight quotes, not curly)
- Inconsistent list formatting

### Technical Issues

- Outdated API references
- Deprecated code patterns
- Missing using statements
- Non-functional code samples
- Inaccurate configuration examples
- Version mismatches

### Formatting

- Missing blank lines between sections
- Incorrect header levels (H1, H2, H3)
- Inconsistent list markers
- Missing language tags in code fences
- Inconsistent capitalization in headers

---

## Review Output Format

When reviewing documentation, provide:

1. **Summary**: High-level assessment of quality
2. **Critical Issues**: Errors that must be fixed (broken code, wrong API)
3. **Improvements**: Editorial suggestions (spelling, grammar, style)
4. **Technical Updates**: Outdated patterns or versions
5. **Suggestions**: Optional enhancements for clarity

Format suggestions clearly with context:

```markdown
## Review of: /path/to/document.md

### Critical Issues
- Line 45: Code sample references non-existent method `GetMessageAsync()`
  - Suggestion: Should be `GetMessage()` based on API docs

### Improvements
- Line 23: "colour" should be "color" (en-US)
- Line 78: Change passive voice to active for clarity
- Section "Advanced Usage" should include error handling example

### Technical Updates
- Line 100: Using obsolete `.ConfigureAwait(false)` pattern
  - Current guidance: Async method should use `await` directly

### Suggestions
- Add troubleshooting section for common timeout errors
- Consider adding a quick-start code snippet
```

---

## Review Focus Areas

### New Documentation

Prioritize:
- Technical accuracy (does it work?)
- Completeness (are there gaps?)
- Clarity for new users
- Code sample validity
- Integration with existing docs

### Updated Documentation

Check:
- What changed and why
- Previous issues resolved
- New problems introduced
- Consistency with surrounding content
- Version/date stamps updated

### Legacy Documentation

Evaluate:
- Technical currency (is it still valid?)
- Modernization opportunities
- Obsolete content removal
- Code sample updates to current patterns
- Outdated references to deprecated features

---

## Local Validation

Before submitting documentation changes, run the docs tool locally to catch issues that would fail CI:

**Troubleshooting:** If you encounter weird unrelated errors, there may be old files on disk. Try:
```bash
git clean -dxf
```

**Install Particular.DocsTool globally:**
```bash
dotnet tool install -g Particular.DocsTool --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
```

**Update the tool (if already installed):**
```bash
dotnet tool update -g Particular.DocsTool --add-source https://f.feedz.io/particular-software/packages/nuget/index.json
```

**Run validation:**
```bash
docstool test
```

This runs the same checks that CI performs on pull requests. Always verify locally before submitting.
