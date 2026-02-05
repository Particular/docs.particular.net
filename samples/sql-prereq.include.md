Ensure an instance of SQL Server (Version 2016 or above for custom saga finders sample, or Version 2012 or above for other samples) is installed and accessible on `localhost` and port `1433`. A Docker image can be used to accomplish this by running `docker run --name mssql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:latest` in a terminal.

Alternatively, change the connection string to point to different SQL Server instance.

At startup each endpoint will create its required SQL assets including databases, tables, and schemas.
