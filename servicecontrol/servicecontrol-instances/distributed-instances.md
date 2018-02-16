---
title: Multiple ServiceControl Instances
reviewed: 2017-07-16
component: ServiceControl
---

### Known Constraints / Issues

- Only supports Audits
- SC Master instances must be configured with Address and API URI of remotes - JSON string
- Paging for message view in SI is wonky
- Data from remote instances that cannot be reached by the master instance will not be included in the results

### Disabling Recoverability

- Set Error queue name to `!disabled`
- Error forwarding, if enabled, will be ignored.
