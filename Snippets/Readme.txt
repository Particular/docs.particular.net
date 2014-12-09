To update
Update-Package NServiceBus -Safe


get-project -all | get-package | ?{ $_.Id -like 'NServiceBus*' } | update-package  -Safe