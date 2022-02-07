USE [AdventureWorks2012]
GO
IF EXISTS (SELECT name FROM sys.tables WHERE name = N'UserRole' and schema_id=(SELECT schema_id FROM sys.schemas WHERE name = N'Security'))
BEGIN 
	drop table [Security].[UserRole]
END
GO

IF EXISTS (SELECT name FROM sys.tables WHERE name = N'User' and schema_id=(SELECT schema_id FROM sys.schemas WHERE name = N'Security'))
BEGIN 
	drop table [Security].[User]
END

GO

IF EXISTS (SELECT name FROM sys.schemas WHERE name = N'Security')
BEGIN 
	drop schema [Security]
END
GO
CREATE SCHEMA Security

GO
/****** Object:  Table [Security].[User]    Script Date: 8/4/2016 4:34:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Security].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](25) NOT NULL,
	[Password] [varchar](128) NOT NULL,
	[Salt] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Security.User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [Security].[UserRole]    Script Date: 8/4/2016 4:34:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Security].[UserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Role] [varchar](25) NOT NULL,
 CONSTRAINT [PK_Security.UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
ALTER TABLE [Security].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User_UserID] FOREIGN KEY([UserId])
REFERENCES [Security].[User] ([UserId])
GO
ALTER TABLE [Security].[UserRole] CHECK CONSTRAINT [FK_UserRole_User_UserID]
GO

INSERT [Security].[User] (UserName, [Password], Salt) VALUES ('admin', 'x+QLcEFLhwDIL+sr7FS5RQ==', '3ma4HDaD' ) 
GO
INSERT [Security].[UserRole] (UserId, [Role]) VALUES ((select UserId from [Security].[User] where UserName = 'admin'), 'CustomerManager')
GO

INSERT [Security].[UserRole] (UserId, [Role]) VALUES ((select UserId from [Security].[User] where UserName = 'admin'), 'ProductManager')
GO

INSERT [Security].[User] (UserName, [Password], Salt) VALUES ('customer', 'ovCEsBTSiLY2daL4V8ta/A==', 'k9JreryF' ) 
GO
INSERT [Security].[UserRole] (UserId, [Role]) VALUES ((select UserId from [Security].[User] where UserName = 'customer'), 'CustomerManager')
GO

