* **Rule ID**: NSB0004, NSB0018
* **Severity**: Warning (NSB0004), Info (NSB0018)
include: simplify-warning-description

The diagnostics NSB0004 and NSB0018 are the same but are applied in different contexts. There is no duplication when only one mapping expression exists, so NSB0018 is presented at level Error. When two or more mapping expressions exist, duplication is present, so NSB0004 is presented as a Error.
include: simplify-warning-description