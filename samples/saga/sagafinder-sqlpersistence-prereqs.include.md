## Prerequisites

### MS SQL Server

include: sql-prereq

### MySQL

Ensure an instance of MySQL (Version 5.7 or above) is installed and accessible on `localhost` and port `3306`. A Docker image can be used to accomplish this by running `docker run --name mysql -e 'MYSQL_ROOT_PASSWORD=yourStrong(!)Password' -e 'MYSQL_DATABASE=sqlpersistencesample' -p 3306:3306 -d mysql:latest` in a terminal.

Alternatively, change the connection string to point to different MySQL instance.

At startup each endpoint will create the required SQL assets including databases, tables, and schemas.

### PostgreSQL

Ensure an instance of PostgreSQL (Version 10 or later) is installed and accessible on `localhost` and port `5432`. A Docker image can be used to accomplish this by running `docker run --name postgres -e 'POSTGRES_PASSWORD=yourStrong(!)Password' -e 'POSTGRES_DB=NsbSamplesSqlSagaFinder' -p 5432:5432 -d postgres:latest` in a terminal.

Alternatively, change the connection string to point to different PostgreSQL instance.

At startup each endpoint will create the required SQL assets including databases, tables, and schemas.
