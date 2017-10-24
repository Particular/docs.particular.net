## Prerequisites


### MS SQL Server

 1. Ensure an instance of SQL Server Express (Version 2016 or above for custom saga finders sample, or Version 2012 or above for other samples) is installed and accessible as `.\SqlExpress`.
 
Or, alternatively, change the connection string to point to different SQL Server instance.

At startup each endpoint will create its required SQL assets including databases, tables and schemas.


### MySQL

 1. Ensure an instance of MySQL (Version 5.7 or above) is installed and accessible on `localhost` and port `3306`.
 1. Add the username to access the instance to an environment variable named `MySqlUserName`.
 1. Add the password to access the instance to an environment variable named `MySqlPassword`.

Or, alternatively, change the connection string to point to different MySQL instance.


### Oracle

 1. Ensure an instance of Oracle Database (Version 11g or later) is installed and accessible on `localhost` on port `1521` with service name `XE`.
 1. Add the username to access the instance to an environment variable named `OracleUserName`.
 1. Add the password to access the instance to an environment variable named `OraclePassword`.

Or, alternatively, change the connection string to point to different Oracle instance.


### PostgreSQL

 1. Ensure an instance of PostgreSQL (Version 10 or later) is installed and accessible on `localhost`.
 1. Add the username to access the instance to an environment variable named `PostgreSqlPassword`.
 1. Add the password to access the instance to an environment variable named `OraclePassword`.

Or, alternatively, change the connection string to point to different PostgreSQL instance.
