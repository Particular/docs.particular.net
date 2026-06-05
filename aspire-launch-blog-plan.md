---
title: Aspire launch blog post
summary: Describes the plan for writing an aspire launch blog post
reviewed: 2026-05-21
component: Aspire.Hosting
---

# Blog plan: Particular Platform Aspire hosting integration

A plan for a launch blog post announcing `Particular.Aspire.Hosting.ServicePlatform`. Built from the launch-blog template (Particular/Exploration), the `particular-story-writing` skill, and the existing Aspire docs in this repo.

## What's being launched

The `Particular.Aspire.Hosting.ServicePlatform` NuGet package — an Aspire hosting integration that adds the full Particular Platform (ServiceControl error/audit/monitoring instances, ServicePulse, managed RavenDB persistence, license, transport) to an Aspire AppHost with one `AddParticularPlatform(...)` call, and lets NServiceBus endpoints attach with `WithParticularPlatform(...)`.

Currently supported (per [platform/aspire/index.md](platform/aspire/index.md)):

- Transports: Learning (dev-only), Azure Service Bus, RabbitMQ, Amazon SQS
- Persistence: RavenDB (managed or external)
- Four samples ship alongside: [platform (Learning)](samples/aspire/platform/sample.md), [platform-asb](samples/aspire/platform-asb/sample.md), [platform-rabbitmq](samples/aspire/platform-rabbitmq/sample.md), [platform-sqs](samples/aspire/platform-sqs/sample.md)

## Story type and audience

**Story type:** Marketing (per [skill story types](file:///C:/DEV/Repos/Website/.github/skills/particular-story-writing/references/story-types.md)). Technical theme, conversational style, CTA at the end. This is a launch post for a new component — closer to Marketing than the short-form Announcements type, because the value isn't self-evident from the package name and needs a few paragraphs to land.

**Primary audience:** .NET developers and technical leads who already use NServiceBus and the Particular Platform, who run a distributed system locally during development, and who are already on (or evaluating) Aspire as their AppHost orchestration layer.

**Secondary audience:** Aspire adopters who haven't yet picked a messaging/observability stack and are evaluating Particular.

**What the reader walks away with:** They know the package exists, they understand the one specific pain it removes (manual platform wiring per environment), and they have one snippet they can copy to try it.

## The hook / the problem

Currently, getting the Particular Platform running alongside an NServiceBus solution locally means hand-rolling the supporting cast:

- Pulling and running the ServiceControl error, audit, and monitoring container images
- Running ServicePulse and pointing it at the right ServiceControl URLs
- Standing up RavenDB for ServiceControl's persistence
- Threading the transport connection string through every ServiceControl instance *and* every endpoint
- Doing all of that again for each developer, each CI environment, each deployment target

Aspire users have already adopted an AppHost to compose their app's own containers and projects. The platform sits outside that — a separate `docker-compose.yml`, a separate readme, a separate "did you start RavenDB?" Slack message.

This is the lede: **the platform now plugs into the same AppHost you already use for everything else.**

## The angle (why this matters, not just what it does)

Three concrete claims worth defending in the post — pick a side, no hedging (per [ai-writing-tells.md](file:///C:/DEV/Repos/Website/.github/skills/particular-story-writing/references/ai-writing-tells.md)):

1. **Same code path, dev to deploy.** The AppHost that runs on the developer's laptop is the same one that produces the publish manifest. No second source of truth for "how does the platform get stood up here?"
2. **Transport, license, and persistence are configured once.** A `WithTransport*` call on the platform resource propagates to every ServiceControl instance *and* every endpoint that calls `WithParticularPlatform(platform)`. Endpoints don't need their own platform package reference — just the env vars Aspire injects.
3. **Free observability on the way in.** Because everything runs as Aspire resources, the dashboard shows nested health, URLs, logs, traces, and metrics for every platform component — including NServiceBus traces and metrics if the service defaults wire OpenTelemetry in (the samples do).

## Outline

Hybrid of the launch-blog template and the skill's outline (Intro → Problem → Solution → How things are better → But what about → Summary). Approximate word counts to hit the skill's 1200–2000 target for a marketing post.

### 1. Intro (~120 words, above `<!-- more -->`)

Open on the friction: setting up the platform alongside an Aspire app today means a `docker-compose.yml` next to your AppHost and a coordination problem between them. Name the package and what it does in one sentence (general availability, not a preview — so no "new Preview" framing). End the preview with the takeaway: one `AddParticularPlatform(...)` call replaces all of that wiring, locally and at deploy time. Link to the [platform/aspire docs](platform/aspire/index.md) and the package on NuGet (leave `// TODO` for the Pardot redirect, link bare NuGet URL for now).

### 2. What's in the box (~150 words)

A short paragraph naming what the platform resource owns (ServiceControl error/audit/monitoring, ServicePulse, RavenDB, transport, license) and how it shows up in the dashboard as a single nested resource. Link to the `AddParticularPlatform` section of the docs.

### 3. Getting started (~200 words, code-led)

One paragraph framing → minimal snippet (taken from or modeled on `aspire-quick-start-1` and `aspire-quick-start-2` snippets used in [platform/aspire/index.md](platform/aspire/index.md)) showing:

```csharp
var platform = builder
    .AddParticularPlatform("particular")
    .AddDefaultComponents();

builder.AddProject<Projects.Sales>("sales")
    .WithParticularPlatform(platform);
```

Then a sentence on what the developer actually sees when they hit F5: dashboard, ServicePulse link, traces flowing. Do **not** end the section on the snippet — per skill's "never close a section with a code snippet" rule, add a sentence after.

### 4. The good stuff: same code, dev to deploy (~250 words)

Argue claim 1 from the angle. Local F5 uses Learning transport with Docker-hosted Raven; production swaps in `WithTransportAzureServiceBus(...)` (or Rabbit, or SQS) and an existing Raven, but it's the same AppHost project. Mention that `aspire publish` / `aspire deploy` carry it forward, and call out the explicit guardrail: Learning transport is blocked in publish mode by design (a small but reassuring detail for skeptics).

### 5. Endpoints attach in one line (~200 words)

Argue claim 2. Show how `WithParticularPlatform(platform)` injects `PARTICULARSOFTWARE_LICENSE` and `ConnectionStrings__<transport-name>` so endpoint projects don't need their own platform reference. Brief mention that the endpoint still configures NServiceBus itself — this is wiring, not magic.

### 6. The Aspire dashboard, applied (~150 words)

Argue claim 3. The platform appears as one parent resource with children nested under it, each surfacing its primary URL. Health rolls up: starts in `Starting`, transitions to `Running` when every child is healthy. With OpenTelemetry in service defaults (the samples set this up via `add-nsb-otel`), NServiceBus traces and metrics show up in the same dashboard view alongside everything else. This is the "free observability" payoff.

### 7. What about my transport? (~200 words)

The "But what about..." section from the skill outline. Cover the supported matrix:

- Learning, Azure Service Bus, RabbitMQ, Amazon SQS — supported today
- Azure Storage Queues, SQL Server, PostgreSQL, IBM MQ — not yet
- Persistence: RavenDB only today

Link to each of the four samples for the supported transports. Acknowledge this is shipping in stages, then point readers at the [shape-the-future/aspire](shape-the-future/aspire.md) page and the [feedback issue](https://github.com/Particular/NServiceBus/issues/6941) so missing-transport readers have somewhere to land that's not the back button.

### 8. Closing CTA (~100 words)

Per template's "Ending Section": reiterate the package name and where it lives — `Particular.Aspire.Hosting.ServicePlatform` on NuGet (Pardot redirect), and the [docs](platform/aspire/index.md). One sentence each to the four samples. End on a short, direct line — no "in conclusion."

## How I'm handling the template's webinar CTA

The launch-blog template suggests a mid-post CTA to a Preview post-launch webinar. This launch is GA, not a Preview, and there's no webinar. The mid-post CTA slot is filled by section 7's link to the [shape-the-future/aspire page](shape-the-future/aspire.md) and the [feedback issue](https://github.com/Particular/NServiceBus/issues/6941) — readers whose transport isn't supported yet get a place to go that isn't the back button. That preserves the template's intent (a CTA somewhere in the middle of the post) without inventing an event.

## Front matter to fill in

Per the launch-blog template and the skill's pre-publish checklist:

| Field | Value | Status |
|---|---|---|
| `title` | Snappy. Working candidates below. | Decide |
| `date` | Launch date | `// TODO` |
| `permalink` | Snake-case of title | Derive |
| `author` | One or more TF members | `// TODO` |
| `bio` | Min. 100 chars each, ideally tied to the post topic | `// TODO` |
| `topics` | `aspire, nservicebus, particular-platform, distributed-systems, dotnet, orchestration` | Draft |
| `PreviewImage.url` | `/images/blog/2026/<slug>.jpg` | `// TODO` |
| `keywords` (per skill) | Same as topics; refine for SEO | Draft |
| `tweets` | Optional per skill | `// TODO` |

Working title candidates (pick or replace):

1. **One AppHost, one platform, no more docker-compose**
2. **The Particular Platform now ships as an Aspire integration**
3. **Stop wiring the platform by hand: `AddParticularPlatform` in Aspire**

Lead recommendation: #1 — it leads on the pain it removes, which is the lede the skill's "don't bury the lede" rule asks for.

## Voice and style commitments (from the skill)

- American English, Oxford comma, contractions
- Active voice; vary sentence length; short punchy sentences end sections
- Real domain examples in code (`Sales`, `Billing` — not `Foo`, `Service1`), no `var`, never end a section on a code block
- No AI tells: no "leverage," "seamless," "robust," "streamline," "in today's fast-paced," "let's dive in," "in conclusion." See [ai-writing-tells.md](file:///C:/DEV/Repos/Website/.github/skills/particular-story-writing/references/ai-writing-tells.md) for the full ban list
- Headings followed by prose, never directly by a list or another heading
- No heading deeper than `h2`
- Pre-publish: meta description (3 candidates under 160 chars, pick best), question-based H2s where natural, alt text on images

## Decisions (from you)

1. **GA, not Preview.** Drop the launch-blog template's "new Preview" framing. The intro names it as a generally available component.
2. **No webinar.** The mid-post CTA target is the [shape-the-future/aspire page](shape-the-future/aspire.md) plus the [feedback issue](https://github.com/Particular/NServiceBus/issues/6941). Folded into section 7.
3. **Publish date** — `// TODO` in the front matter; you'll fill in before publish.
4. **Authors / bios** — `// TODO` per author block in the front matter.
5. **Preview image** — `// TODO` for `PreviewImage.url` and `altText`. I'll suggest a couple of royalty-free options when delivering the draft so you have a starting point.
6. **NuGet / Pardot** — `// TODO` next to the bare NuGet link wherever it appears.
7. **Draft destination** — `C:\DEV\Repos\Website\source\_posts\2026\<date>-<permalink>.md`. I'll match the existing filename convention in that folder when I create the file.

## Progress checklist

Mirrors the skill's 6-step Write Workflow, scoped to this post. Tick items as they're done.

### Setup

- [x] Inspect `C:\DEV\Repos\Website\source\_posts\2026\` to confirm filename convention (no date prefix — `<slug>.md` inside year folder, per existing 2025 posts)
- [x] Create draft file at `C:\DEV\Repos\Website\source\_posts\2026\one-apphost-one-platform-no-more-docker-compose.md`

### Front matter

- [x] `title` — "One AppHost, one platform, no more docker-compose"
- [x] `date` — `// TODO` placeholder (2026-05-29) until launch date is set
- [x] `permalink` — derived as `one-apphost-one-platform-no-more-docker-compose` (Hexo derives from filename, no explicit field needed)
- [x] `author` block — `// TODO` placeholder
- [x] `bio` per author (≥100 chars, tied to topic) — `// TODO` placeholder
- [x] `topics` / `keywords` — both filled in
- [x] `previewImage.url` and `altText` — `// TODO` placeholder; image suggestions in Handoff
- [x] `tweets` — left out (optional per skill)
- [x] Meta description: drafted 3 candidates under 160 chars, recommended pick — see Handoff

### Draft body (skill Step 3, one or two bullets at a time)

- [x] Section 1 — Intro and `<!-- more -->` break
- [x] Section 2 — What does the platform resource own?
- [x] Section 3 — How do I add it to an AppHost? (with quick-start snippets)
- [x] Section 4 — Same code from F5 to deploy
- [x] Section 5 — Endpoints attach in one line
- [x] Section 6 — What does it look like in the dashboard?
- [x] Section 7 — What about my transport? + shape-the-future CTA
- [x] Section 8 — Try it (closing CTA)

### Pre-Publish Checklist (skill Step 4)

- [x] Front matter: bio ≥100 chars marked `// TODO`, keywords + topics filled, previewImage + altText marked `// TODO`, date marked `// TODO`
- [x] Preview text above `<!-- more -->` reads well in isolation (sets up the package, the pain, and the F5-to-deploy payoff)
- [x] Relevant CTAs present at the end (NuGet, docs, samples, shape-the-future)
- [x] Further reading / related links included
- [x] External links route to blog or docs (NuGet + Aspire.dev are the only off-platform links)
- [x] No heading deeper than `h2`
- [x] Word count ~1480, inside the 1200–2000 range
- [x] Common-questions section — "What about my transport?" serves this role
- [x] Alt text marker present on preview image (`// TODO` until image is picked)
- [x] Maps to a clear job-to-be-done ("get the Particular Platform into my Aspire AppHost")

### Readability pass (skill Step 5)

- [x] Passive voice → active where possible (a couple of passive structures left where they describe ongoing process — "is being built out in stages")
- [x] Long sentences shortened; each section ends on a short, punchy line
- [x] "very," "really," "quite" — none present; only `just` remains, in a conversational construction
- [x] Jargon checked against a developer new to Aspire — Aspire concepts introduced before use, NServiceBus terms named directly
- [x] AI-tells scan against the ai-writing-tells.md ban list — no hits after the two "No X. No Y. No Z." patterns were rewritten
- [x] No section closes on a code snippet
- [x] No heading directly followed by a list or another heading
- [x] Sentence-length rhythm varied — fragments and long sentences interleaved deliberately

### Finalize delivery (skill Step 6)

- [x] Image picked — not yet. Three royalty-free suggestions listed in Handoff for the user to pick from.
- [x] All internal links — verified against the docs paths read during prep (`platform/aspire/index.md`, `shape-the-future/aspire.md`, four sample paths)
- [x] NuGet link — bare URL in the post; `// TODO` comments mark both occurrences for Pardot wrapping
- [x] Meta description — three candidates listed in Handoff, recommendation marked

### Handoff

- [x] Final markdown saved at `C:\DEV\Repos\Website\source\_posts\2026\one-apphost-one-platform-no-more-docker-compose.md`
- [x] Outstanding `// TODO`s summarized for the user (see Handoff section below)

## Handoff

### Outstanding `// TODO`s

The draft is complete and reads end-to-end, but the following placeholders need real values before publish. Each is marked with `// TODO` in the file so they're easy to find:

1. **`date`** (front matter) — currently `2026-05-29`. Set to the launch date.
2. **`author`** + **`bio`** (front matter) — replace `TODO_AUTHOR` with one or more TF member handles and write a 100+ character bio for each, ideally tied to Aspire / distributed dev.
3. **`previewImage.url`**, **`altText`**, **`from-where`** (front matter) — pick an image, add it to `/images/blog/2026/`, fill in the three fields. See suggestions below.
4. **NuGet Pardot redirect** — two places in the body link to `https://www.nuget.org/packages/Particular.Aspire.Hosting.ServicePlatform`. Both are flagged with `<!-- TODO: wrap NuGet URL in Pardot redirect -->`. Replace once the Pardot link is generated.

### Meta description — three candidates (pick one for the front matter)

| # | Candidate | Chars |
| --- | --- | --- |
| 1 | `Particular.Aspire.Hosting.ServicePlatform adds ServiceControl, ServicePulse, and managed RavenDB to your Aspire AppHost with one line. F5 to deploy, same code.` | 157 |
| 2 | `One AddParticularPlatform() call brings ServiceControl, ServicePulse, and RavenDB into your Aspire AppHost. No second compose file, dev to deploy.` | 147 |
| 3 | `Run the whole Particular Platform inside your Aspire AppHost. NServiceBus endpoints attach in one line and pick up the license and transport from env vars.` | 157 |

**Recommended: #2.** Leads with the concrete API (good for SEO), names the three biggest payloads, ends on the dev-to-deploy benefit, and is the shortest. Add this as a `description:` or `meta description:` front-matter field per the Hexo blog convention (the existing 2025 posts don't all carry one, so confirm where the Website repo expects it).

### Preview image — three royalty-free suggestions

The skill points at Pixabay, Snappygoat, and Flickr CC; recent posts have also used AI-generated images. Three concrete starting points:

1. **Pixabay search: "container yard aerial"** — visual metaphor for *everything in one place, organized*. Fits the "one AppHost" lede directly.
2. **Pixabay search: "orchestra conductor"** — fits the orchestration story (Aspire literally orchestrates). The AWS Enhancements 2025 post used a similar concept image.
3. **AI-generated (ChatGPT / Midjourney)** — a cartoon of an AppHost icon with the platform components nested inside it. The AWS 2025 post used the same pattern (`from-where: ChatGPT AI generated`), so there's precedent.

Pick one, add it to `C:\DEV\Repos\Website\source\images\blog\2026\`, then update `previewImage.url`, `altText`, and `from-where` in the front matter.

### What I did NOT do

- **No actual image added.** That's a creative call I left to you.
- **No Pardot redirect generated.** I don't know your process for that.
- **No author/bio.** I don't know who the TF authors are.
- **Did not run the post through Hemingway / Grammarly.** The skill suggests this as Step 5 validation. The AI-tells scan and structural review caught the obvious things, but a Hemingway pass may surface long sentences worth shortening further.

### Quick edit recipe if you want to ship today

1. Set `date` to today.
2. Replace `TODO_AUTHOR` blocks (top of front matter) with real handles + bios.
3. Drop in an image and fill the three `previewImage` fields.
4. Choose a meta description from the three candidates above and add it to the front matter.
5. Delete the two `<!-- TODO: wrap NuGet URL in Pardot redirect -->` comments and swap the bare NuGet URL for the Pardot redirect.

The body itself doesn't need further edits to ship.

## Sources I'll cite from (already read)

- [platform/aspire/index.md](platform/aspire/index.md) — full integration reference; primary source for what the package does and how it's configured
- [shape-the-future/aspire.md](shape-the-future/aspire.md) — the "tell us what's missing" feedback page; target for section 7
- [samples/aspire/platform/sample.md](samples/aspire/platform/sample.md) — Learning transport walkthrough
- [samples/aspire/platform-asb/sample.md](samples/aspire/platform-asb/sample.md) — Azure Service Bus walkthrough
- [samples/aspire/platform-rabbitmq/sample.md](samples/aspire/platform-rabbitmq/sample.md) — RabbitMQ walkthrough
- [samples/aspire/platform-sqs/sample.md](samples/aspire/platform-sqs/sample.md) — Amazon SQS walkthrough

## Process

1. You give the updated plan a final OK (or flag anything still off).
2. Before drafting, I check `C:\DEV\Repos\Website\source\_posts\2026\` for the filename convention and pick the matching pattern for `<date>-<permalink>.md`.
3. I draft per the skill's Step 3 (bullet-by-bullet, one or two paragraphs at a time, leaving `// TODO`s for the items above).
4. I run the draft against the skill's Pre-Publish Checklist (Step 4) and Hemingway-style readability pass (Step 5).
5. I deliver the final markdown at the destination above, with front matter, three meta description candidates under 160 chars, and two or three royalty-free image suggestions.
