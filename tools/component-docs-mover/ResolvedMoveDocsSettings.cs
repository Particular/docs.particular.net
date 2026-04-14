namespace component_docs_mover;

sealed record ResolvedMoveDocsSettings(
    string Component,
    string From,
    string To,
    string RepoRoot,
    string MenuPath,
    bool Apply);
