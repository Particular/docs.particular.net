CREATE DATABASE SqlTransportTest
USE SqlTransportTest
CREATE LOGIN MyUser WITH PASSWORD = 'pass@123';
CREATE USER MyUser FOR LOGIN MyUser;
EXEC sp_addrolemember 'db_owner', 'MyUser'