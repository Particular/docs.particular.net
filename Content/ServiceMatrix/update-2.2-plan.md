# Service Matrix V2.2.0 Documentation Update Plan

## New Features Needing Coverage

1. NSB V4 vs. NSB V5 project templates.
2. SignalR
3. Ability to select existing handler for a new message.
4. Ability to modify an existing saga when subscribing to another event.

## General Documentation Wishlist

1. More independent guidance articles (as opposed to tutorial-based) on common topics such as Request/Response, Pub/Sub, Sagas (Buyer's Remorse example), SignalR, etc.
2. Independent guidance articles on configuration customization.
3. Address the [necessary workaround](https://github.com/Particular/NServiceBus/issues/2437) when you have RavenDB 2.5 installed but are working with an NSB V4 project.

## Tasks

I believe these tasks are in the correct priority order. Tasks 1-3 could be done as one release to get the critical new 2.2 items in the hands of users asap.

1. Update screenshots and code snippets to be consistent with new version **DONE** with exception of RavenDB error issue 
2. Add quick paragraph discussing NSB V4 vs. V5 template to the `getting-started-with-servicematrix-2.0.md` document. **DONE**
3. Add SignalR example to the Getting Started tutorial (likely the last item, after 'Using Sagas in ServiceMatrix') **DONE**
4. Add new topics under Using ServiceMatrix, with simple, self-contained examples for TBD topics (thoughts: Request/Response, Pub/Sub, Working with Sagas, Re-using a message handler, and customizing configuration).
5. Add item for the [NSB V4/RavenDB 2.5 issue](https://github.com/Particular/NServiceBus/issues/2437) to the Troubleshooting section.