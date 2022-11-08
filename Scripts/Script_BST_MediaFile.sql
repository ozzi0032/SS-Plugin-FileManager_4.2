USE [SmartStore42]
GO

/****** Object:  Table [dbo].[BST_MediaFile]    Script Date: 23/05/2022 11:16:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BST_MediaFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[HeaderId] [int] NOT NULL,
	[FilePath] [nvarchar](max) NULL,
	[FileKey] [nvarchar](max) NULL,
	[FileContent] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[Size] [int] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Extension] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[UpdatedOnUtc] [datetime] NULL,
 CONSTRAINT [PK_BST_MediaFile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


