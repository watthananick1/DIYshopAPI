CREATE DATABASE shopDIYdb;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Firstname] [nvarchar](max) NOT NULL,
	[Lastname] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[Proflieimg] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Firstname] [nvarchar](max) NOT NULL,
	[Lastname] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[BridDate] [datetime2](7) NULL,
	[Email] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[N_Id] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[ImgPoduct] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderItems] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Order_Id]      INT             NOT NULL,
    [Product_id]    INT             NOT NULL,
    [N_Id]          NVARCHAR (MAX)  NOT NULL,
    [Item_Price]    DECIMAL (18, 2) NOT NULL,
    [Item_Quantity] INT             NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Orders] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Date]         DATETIME        NOT NULL,
    [OrderId]      NVARCHAR (MAX)  NOT NULL,
    [Total_Price]  DECIMAL (18, 2) NOT NULL,
    [User_Id]      INT             NOT NULL,
    [Customer_Id]  INT             NULL,
    [Promotion_id] INT             NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[PromotionProducts] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [Promotion_Id] INT NOT NULL,
    [Product_Id]   INT NOT NULL,
    CONSTRAINT [PK_PromotionProducts] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Promotions] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [PromotionId]    UNIQUEIDENTIFIER NOT NULL,
    [StartPromotion] DATETIME         NOT NULL,
    [EndPromotion]   DATETIME         NOT NULL,
    [Discount]       DECIMAL (18, 2)  NOT NULL,
    CONSTRAINT [PK_Promotions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

INSERT INTO [dbo].[Users] (
    [UserName],
    [Password],
    [Firstname],
    [Lastname],
    [Status],
    [CreatedDate],
    [Email],
    [PhoneNumber],
    [Proflieimg]
) VALUES (
    'john_doe',
    '$2a$11$nAqTX5q2mz/1A6kVGILlFuMvUEQ8yTZU0Y/YLOXYPH8Flnc1wLD0K',
    'John',
    'Doe',
    1,
    GETDATE(),
    'john.doe@example.com',
    '1234567890',
    'path/to/john_doe_profile.jpg'
);





