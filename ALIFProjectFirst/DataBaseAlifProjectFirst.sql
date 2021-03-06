USE [ALIFProjectFirst]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 30.09.2021 14:58:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[LastName] [nvarchar](30) NOT NULL,
	[PhoneNumber] [int] NOT NULL,
	[SerialNumber] [varchar](9) NOT NULL,
	[TaxPayerIDNumber] [int] NOT NULL,
	[Role_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Credit]    Script Date: 30.09.2021 14:58:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Credit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NULL,
	[SumOfCredit] [decimal](18, 0) NOT NULL,
	[CreditHistory] [int] NOT NULL,
	[DelaysCreitHistory] [int] NOT NULL,
	[CreditTerm] [int] NOT NULL,
	[GoalOfCredit] [nvarchar](30) NOT NULL,
	[Status_Id] [int] NULL,
	[Amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditHistory]    Script Date: 30.09.2021 14:58:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[RangeTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 30.09.2021 14:58:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 30.09.2021 14:58:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkSheet]    Script Date: 30.09.2021 14:58:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkSheet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NULL,
	[Sex] [varchar](5) NOT NULL,
	[MaritalStatus] [varchar](15) NOT NULL,
	[Age] [int] NOT NULL,
	[Nationality] [varchar](15) NOT NULL,
	[Amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Credit]  WITH CHECK ADD FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Credit]  WITH CHECK ADD FOREIGN KEY([Status_Id])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[WorkSheet]  WITH CHECK ADD FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Id])
GO
