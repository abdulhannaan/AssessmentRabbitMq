/****** Object:  Database [DtechLogger]    Script Date: 11/2/2024 3:56:27 AM ******/
CREATE DATABASE [DtechLogger]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DtechLogger', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DtechLogger.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DtechLogger_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DtechLogger_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [DtechLogger] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DtechLogger].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DtechLogger] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DtechLogger] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DtechLogger] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DtechLogger] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DtechLogger] SET ARITHABORT OFF 
GO
ALTER DATABASE [DtechLogger] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DtechLogger] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DtechLogger] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DtechLogger] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DtechLogger] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DtechLogger] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DtechLogger] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DtechLogger] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DtechLogger] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DtechLogger] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DtechLogger] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DtechLogger] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DtechLogger] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DtechLogger] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DtechLogger] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DtechLogger] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DtechLogger] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DtechLogger] SET RECOVERY FULL 
GO
ALTER DATABASE [DtechLogger] SET  MULTI_USER 
GO
ALTER DATABASE [DtechLogger] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DtechLogger] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DtechLogger] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DtechLogger] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DtechLogger] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DtechLogger] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DtechLogger', N'ON'
GO
ALTER DATABASE [DtechLogger] SET QUERY_STORE = ON
GO
ALTER DATABASE [DtechLogger] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/****** Object:  Table [dbo].[myLogger]    Script Date: 11/2/2024 3:56:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[myLogger](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NULL,
	[Originator] [varchar](50) NULL,
	[FileName] [varchar](255) NULL,
	[Status] [varchar](50) NULL,
 CONSTRAINT [PK_myLogger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[myLogger] ON 
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (1, CAST(N'2024-11-02T07:45:50.493' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'Received')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (2, CAST(N'2024-11-02T07:45:50.927' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'SuccessfullyProcessed')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (3, CAST(N'2024-11-02T07:45:50.980' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'SuccessfullyConsumed')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (4, CAST(N'2024-11-02T07:45:50.987' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'FileWritten')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (5, CAST(N'2024-11-02T07:52:16.730' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'Received')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (6, CAST(N'2024-11-02T07:52:17.153' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'SuccessfullyProcessed')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (7, CAST(N'2024-11-02T07:52:17.250' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'SuccessfullyConsumed')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (8, CAST(N'2024-11-02T07:53:00.690' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'Received')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (9, CAST(N'2024-11-02T07:53:01.063' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'SuccessfullyProcessed')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (10, CAST(N'2024-11-02T07:53:01.107' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'SuccessfullyConsumed')
GO
INSERT [dbo].[myLogger] ([Id], [LogDate], [Originator], [FileName], [Status]) VALUES (11, CAST(N'2024-11-02T07:53:01.117' AS DateTime), N'InternetBanking', N'myTestFile.txt', N'FileWritten')
GO
SET IDENTITY_INSERT [dbo].[myLogger] OFF
GO
ALTER DATABASE [DtechLogger] SET  READ_WRITE 
GO
