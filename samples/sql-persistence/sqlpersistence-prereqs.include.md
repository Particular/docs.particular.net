## Prerequisites

### MS SQL Server

include: sql-prereq

### MySQL

Ensure an instance of MySQL (Version 5.7 or above) is installed and accessible on `localhost` and port `3306`. A Docker image can be used to accomplish this by running `docker run --name mysql -e 'MYSQL_ROOT_PASSWORD=yourStrong(!)Password' -p 3306:3306 -d mysql:latest` in a terminal.

Alternatively, change the connection string to point to different MySQL instance.

At startup each endpoint will create the required SQL assets including databases, tables, and schemas.

### Oracle

Ensure an instance of Oracle (Version 11g or later) is installed and accessible on `localhost` and port `1521`. A Docker image can be used to accomplish this by running `docker run --name oracle -e 'ORACLE_PASSWORD=yourStrong(!)Password' -p 1521:1521 -d gvenzl/oracle-free:23-slim` in a terminal.

Alternatively, change the connection string to point to different Oracle instance.

At startup each endpoint will create the required SQL assets including databases, tables, and schemas.

### PostgreSQL

Ensure an instance of PostgreSQL (Version 10 or later) is installed and accessible on `localhost` and port `5432`. A Docker image can be used to accomplish this by running `docker run --name postgres -e 'POSTGRES_PASSWORD=yourStrong(!)Password' -p 5432:5432 -d postgres:latest` in a terminal.

Alternatively, change the connection string to point to different PostgreSQL instance.

At startup each endpoint will create the required SQL assets including databases, tables, and schemas.
