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

## Versioned file names

- Partials are named `{filePrefix}_{key}_{componentAlias}_{versionRange}.partial.md` and live next to the page that includes them. The range uses NuGet interval notation, for example `[10,11)` or `[11,)`.
- When a new major version changes behavior that a partial describes, close the open-ended range of the current partial and add a new partial for the new major: rename `x_key_core_[10,).partial.md` to `x_key_core_[10,11).partial.md` and add `x_key_core_[11,).partial.md`. Ranges of sibling partials must not overlap.
- Write each partial from its own version's reader perspective: that version's defaults and its opt-in options. Do not describe the transition from the previous version there; that belongs in the upgrade guide under `nservicebus/upgrades/`.
- Snippets are versioned by directory (`Snippets/Core/Core_10` covers `[10,11)`) or by a version suffix on the `#region` marker. A `snippet:` key in a partial resolves against the snippet version that matches the page version, so a `[11,)` partial needs its keys in a `Core_11` directory once one exists. Only snippet directories add an entry to the version dropdown; a partial alone does not.
- Use a distinct snippet key when the same setting means something different per version (for example a snippet that sets the non-default value), so that copying a snippet directory to the next major does not silently document the previous major's default.
- Use inline `#if-version [10.3,)` blocks for a short note inside an otherwise shared page; use a partial when the content differs substantially.
- Do not document `DOTNET_*` environment variables as a way to set AppContext switches. The .NET runtime does not map arbitrary environment variables to `AppContext` switches; document `AppContext.SetSwitch` and the `RuntimeHostConfigurationOption` MSBuild item instead.
