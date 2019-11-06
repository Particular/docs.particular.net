Reply messages that have no longer an associated callback will be moved to the error queue.

It is possible to leverage a custom recoverability policy with a discard action to prevent messages from being moved to the error queue.

snippet: Callbacks-Recoverability