USE [SmartStore42]
GO

/****** Object:  Table [dbo].[BST_MediaFile]    Script Date: 23/05/2022 11:16:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BST_Header](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Title] [nvarchar](250) NULL,
	[CRC] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CodeSnippet] [nvarchar](max) NULL,
 CONSTRAINT [PK_BST_Header] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


