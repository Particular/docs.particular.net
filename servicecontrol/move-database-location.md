---
title: ServiceControl Move Database to Another Location
summary: Move Database to Another Location
reviewed: 2025-06-29
---


Move the database to another disk if the disk that isn't following the [ServiceControl storage recommendations](/servicecontrol/servicecontrol-instances/hardware.md#general-recommendations-storage-recommendations).

To move the database to another location:

1. Open the ServiceControl Management utility (SCMU) (via Windows Start)
2. Stop the ServiceControl instance
3. Open Windows services
4. Note the Windows service startup type (Automatic or Manual)
5. Set the Windows service startup type to Disabled to ensure the instance cannot be accidentally started
6. Open the DB or Log path via SCMU and move on level up (i.e. `C:\ProgramData\Particular\ServiceControl\Particular.ServiceControl`)
7. Copy the full storage path to a new disk (i.e. `C:\ProgramData\Particular\ServiceControl\Particular.ServiceControl` to `F:\Particular.ServiceControl`
8. Open the *Installation Path* in Windows Explorer by selecting the installation path browse button
9. Open an elevated text editor (Run as Administrator)
10. Open the file matching `ServiceControl*.exe.config` in the elevated text editor
11. Modify the values for keys `*/DBPath` and `*/LogPath` to match the new location
12. Save the config file in an elevated text editor (if this fails, the editor was not launched to Run as Administrator)
13. Restore the Windows service startup type to its previous state (Automatic or Manual)
14. Start the ServiceControl instance
15. Delete the obsolete files from the previous storage location
