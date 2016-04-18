-- startcode CreateDatabaseForSqlPersistence
USE [master]
GO

CREATE DATABASE [PersistenceForSqlTransport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PersistenceForSqlTransport', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForSqlTransport.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PersistenceForSqlTransport_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForSqlTransport_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
-- endcode