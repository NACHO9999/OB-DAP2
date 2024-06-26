USE [master]
GO
/****** Object:  Database [AppDB]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE DATABASE [AppDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AppDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AppDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AppDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\AppDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AppDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AppDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AppDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AppDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AppDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AppDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [AppDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AppDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AppDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AppDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AppDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AppDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AppDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AppDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AppDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AppDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AppDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [AppDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AppDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AppDB] SET  MULTI_USER 
GO
ALTER DATABASE [AppDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AppDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AppDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AppDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AppDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [AppDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AppDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[Nombre] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Constructoras]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Constructoras](
	[Nombre] [nvarchar](max) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Constructoras] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deptos]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deptos](
	[Numero] [int] NOT NULL,
	[EdificioNombre] [nvarchar](450) NOT NULL,
	[EdificioDireccion] [nvarchar](450) NOT NULL,
	[Piso] [int] NOT NULL,
	[DuenoEmail] [nvarchar](450) NULL,
	[CantidadCuartos] [int] NOT NULL,
	[CantidadBanos] [int] NOT NULL,
	[ConTerraza] [bit] NOT NULL,
 CONSTRAINT [PK_Deptos] PRIMARY KEY CLUSTERED 
(
	[Numero] ASC,
	[EdificioNombre] ASC,
	[EdificioDireccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Duenos]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Duenos](
	[Email] [nvarchar](450) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Apellido] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Duenos] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Edificios]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Edificios](
	[Nombre] [nvarchar](450) NOT NULL,
	[Direccion] [nvarchar](450) NOT NULL,
	[Ubicación] [nvarchar](max) NOT NULL,
	[GastosComunes] [decimal](18, 2) NOT NULL,
	[EncargadoEmail] [nvarchar](450) NULL,
	[EmpresaConstructoraId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Edificios] PRIMARY KEY CLUSTERED 
(
	[Nombre] ASC,
	[Direccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invitaciones]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invitaciones](
	[Email] [nvarchar](450) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[FechaExpiracion] [datetime2](7) NOT NULL,
	[Rol] [int] NOT NULL,
 CONSTRAINT [PK_Invitaciones] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthToken] [uniqueidentifier] NOT NULL,
	[UsuarioEmail] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Solicitudes]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitudes](
	[Id] [uniqueidentifier] NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[DeptoNumero] [int] NOT NULL,
	[DeptoEdificioNombre] [nvarchar](450) NOT NULL,
	[DeptoEdificioDireccion] [nvarchar](450) NOT NULL,
	[CategoriaNombre] [nvarchar](450) NOT NULL,
	[Estado] [int] NOT NULL,
	[FechaInicio] [datetime2](7) NOT NULL,
	[FechaFin] [datetime2](7) NULL,
	[PerManEmail] [nvarchar](450) NULL,
 CONSTRAINT [PK_Solicitudes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 6/10/2024 7:11:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Email] [nvarchar](450) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Apellido] [nvarchar](max) NOT NULL,
	[Contrasena] [nvarchar](max) NOT NULL,
	[Discriminator] [nvarchar](21) NOT NULL,
	[ConstructoraId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Deptos_DuenoEmail]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Deptos_DuenoEmail] ON [dbo].[Deptos]
(
	[DuenoEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Deptos_EdificioNombre_EdificioDireccion]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Deptos_EdificioNombre_EdificioDireccion] ON [dbo].[Deptos]
(
	[EdificioNombre] ASC,
	[EdificioDireccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Edificios_EmpresaConstructoraId]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Edificios_EmpresaConstructoraId] ON [dbo].[Edificios]
(
	[EmpresaConstructoraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Edificios_EncargadoEmail]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Edificios_EncargadoEmail] ON [dbo].[Edificios]
(
	[EncargadoEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Sessions_UsuarioEmail]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Sessions_UsuarioEmail] ON [dbo].[Sessions]
(
	[UsuarioEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Solicitudes_CategoriaNombre]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Solicitudes_CategoriaNombre] ON [dbo].[Solicitudes]
(
	[CategoriaNombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Solicitudes_DeptoNumero_DeptoEdificioNombre_DeptoEdificioDireccion]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Solicitudes_DeptoNumero_DeptoEdificioNombre_DeptoEdificioDireccion] ON [dbo].[Solicitudes]
(
	[DeptoNumero] ASC,
	[DeptoEdificioNombre] ASC,
	[DeptoEdificioDireccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Solicitudes_PerManEmail]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Solicitudes_PerManEmail] ON [dbo].[Solicitudes]
(
	[PerManEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Usuarios_ConstructoraId]    Script Date: 6/10/2024 7:11:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Usuarios_ConstructoraId] ON [dbo].[Usuarios]
(
	[ConstructoraId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Constructoras] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [Id]
GO
ALTER TABLE [dbo].[Edificios] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [EmpresaConstructoraId]
GO
ALTER TABLE [dbo].[Invitaciones] ADD  DEFAULT ((0)) FOR [Rol]
GO
ALTER TABLE [dbo].[Deptos]  WITH CHECK ADD  CONSTRAINT [FK_Deptos_Duenos_DuenoEmail] FOREIGN KEY([DuenoEmail])
REFERENCES [dbo].[Duenos] ([Email])
GO
ALTER TABLE [dbo].[Deptos] CHECK CONSTRAINT [FK_Deptos_Duenos_DuenoEmail]
GO
ALTER TABLE [dbo].[Deptos]  WITH CHECK ADD  CONSTRAINT [FK_Deptos_Edificios_EdificioNombre_EdificioDireccion] FOREIGN KEY([EdificioNombre], [EdificioDireccion])
REFERENCES [dbo].[Edificios] ([Nombre], [Direccion])
GO
ALTER TABLE [dbo].[Deptos] CHECK CONSTRAINT [FK_Deptos_Edificios_EdificioNombre_EdificioDireccion]
GO
ALTER TABLE [dbo].[Edificios]  WITH CHECK ADD  CONSTRAINT [FK_Edificios_Constructoras_EmpresaConstructoraId] FOREIGN KEY([EmpresaConstructoraId])
REFERENCES [dbo].[Constructoras] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Edificios] CHECK CONSTRAINT [FK_Edificios_Constructoras_EmpresaConstructoraId]
GO
ALTER TABLE [dbo].[Edificios]  WITH CHECK ADD  CONSTRAINT [FK_Edificios_Usuarios_EncargadoEmail] FOREIGN KEY([EncargadoEmail])
REFERENCES [dbo].[Usuarios] ([Email])
GO
ALTER TABLE [dbo].[Edificios] CHECK CONSTRAINT [FK_Edificios_Usuarios_EncargadoEmail]
GO
ALTER TABLE [dbo].[Sessions]  WITH CHECK ADD  CONSTRAINT [FK_Sessions_Usuarios_UsuarioEmail] FOREIGN KEY([UsuarioEmail])
REFERENCES [dbo].[Usuarios] ([Email])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sessions] CHECK CONSTRAINT [FK_Sessions_Usuarios_UsuarioEmail]
GO
ALTER TABLE [dbo].[Solicitudes]  WITH CHECK ADD  CONSTRAINT [FK_Solicitudes_Categorias_CategoriaNombre] FOREIGN KEY([CategoriaNombre])
REFERENCES [dbo].[Categorias] ([Nombre])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Solicitudes] CHECK CONSTRAINT [FK_Solicitudes_Categorias_CategoriaNombre]
GO
ALTER TABLE [dbo].[Solicitudes]  WITH CHECK ADD  CONSTRAINT [FK_Solicitudes_Deptos_DeptoNumero_DeptoEdificioNombre_DeptoEdificioDireccion] FOREIGN KEY([DeptoNumero], [DeptoEdificioNombre], [DeptoEdificioDireccion])
REFERENCES [dbo].[Deptos] ([Numero], [EdificioNombre], [EdificioDireccion])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Solicitudes] CHECK CONSTRAINT [FK_Solicitudes_Deptos_DeptoNumero_DeptoEdificioNombre_DeptoEdificioDireccion]
GO
ALTER TABLE [dbo].[Solicitudes]  WITH CHECK ADD  CONSTRAINT [FK_Solicitudes_Usuarios_PerManEmail] FOREIGN KEY([PerManEmail])
REFERENCES [dbo].[Usuarios] ([Email])
GO
ALTER TABLE [dbo].[Solicitudes] CHECK CONSTRAINT [FK_Solicitudes_Usuarios_PerManEmail]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Constructoras_ConstructoraId] FOREIGN KEY([ConstructoraId])
REFERENCES [dbo].[Constructoras] ([Id])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Constructoras_ConstructoraId]
GO
USE [master]
GO
ALTER DATABASE [AppDB] SET  READ_WRITE 
GO
