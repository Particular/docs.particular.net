#### CommitDelayIncrement

The time increment used to delay the commit of the transactional session when the outbox record is not yet in the storage (more details see [Phase 2](#how-it-works-phase-2)).

The default value for the commit delay increment is `TimeSpan.FromSeconds(2)`.

snippet: configuring-commit-delay-transactional-session
