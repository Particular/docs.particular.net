
-- startcode CreateDatabaseForSqlPersistence
USE [master]
GO

/****** Object:  Database [PersistenceForSqlTransport]    Script Date: 2/9/2016 11:57:44 AM ******/
CREATE DATABASE [PersistenceForSqlTransport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PersistenceForSqlTransport', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForSqlTransport.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PersistenceForSqlTransport_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForSqlTransport_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [PersistenceForSqlTransport] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PersistenceForSqlTransport].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [PersistenceForSqlTransport] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET ARITHABORT OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET  DISABLE_BROKER 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET  MULTI_USER 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [PersistenceForSqlTransport] SET DB_CHAINING OFF 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [PersistenceForSqlTransport] SET  READ_WRITE 
GO



-- endcode