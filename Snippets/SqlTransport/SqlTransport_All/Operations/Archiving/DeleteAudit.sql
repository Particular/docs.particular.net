--startcode delete-audit

delete from [dbo].[audit]
output [deleted].*
into [dbo].[audit_archive]

--endcode