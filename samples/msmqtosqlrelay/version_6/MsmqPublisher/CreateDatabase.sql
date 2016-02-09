
-- startcode CreateDatabase
USE [master]
GO

/****** Object:  Database [PersistenceForMsmqTransport]    Script Date: 2/9/2016 10:20:43 AM ******/
CREATE DATABASE [PersistenceForMsmqTransport]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PersistenceForMsmqTransport', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForMsmqTransport.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PersistenceForMsmqTransport_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PersistenceForMsmqTransport_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PersistenceForMsmqTransport].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET ARITHABORT OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET  ENABLE_BROKER 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET  MULTI_USER 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET DB_CHAINING OFF 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [PersistenceForMsmqTransport] SET  READ_WRITE 
GO
-- endcode