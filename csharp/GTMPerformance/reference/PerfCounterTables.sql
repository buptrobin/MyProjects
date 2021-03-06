/****************************************************************************************/
--
-- Create Performance Counter Category table
--
/****************************************************************************************/

/****** Object:  Table [dbo].[PerfCounterCategory]    Script Date: 01/01/2009 20:13:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerfCounterCategory]') AND type in (N'U'))
DROP TABLE [dbo].[PerfCounterCategory]
GO
/****** Object:  Table [dbo].[PerfCounterCategory]    Script Date: 01/01/2009 20:13:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerfCounterCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PerfCounterCategory](
    [CategoryName] [nvarchar](50) NOT NULL,
    [CounterName] [nvarchar](50) NOT NULL,
    [CounterType] [nchar](16) NOT NULL,
    [Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_PerfCounterCategory] PRIMARY KEY CLUSTERED 
(
    [CategoryName] ASC,
    [CounterName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****************************************************************************************/
--
-- Create Performance Counter Category: insert category data into table:
-- A better practice is to define the perf category in a text file, then bulk load to DB.
--
/****************************************************************************************/

INSERT INTO [PerfCounterCategory] VALUES ('SeMSJobProc', 'PendingJobs', 'int', NULL);
INSERT INTO [PerfCounterCategory] VALUES ('SeMSJobProc', 'ProcessingJobs', 'int', NULL);
INSERT INTO [PerfCounterCategory] VALUES ('SeMSJobProc', 'FailedJobs', 'int', NULL);
-- INSERT INTO [PerfCounterCategory] VALUES ()
-- INSERT INTO [PerfCounterCategory] VALUES ()
-- INSERT INTO [PerfCounterCategory] VALUES ()
-- INSERT INTO [PerfCounterCategory] VALUES ()
-- INSERT INTO [PerfCounterCategory] VALUES ()
-- INSERT INTO [PerfCounterCategory] VALUES ()


/****************************************************************************************/
--
-- Create Performance Counter Instance table
--
/****************************************************************************************/
/****** Object:  Table [dbo].[PerfCounterInstance]    Script Date: 01/01/2009 20:14:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerfCounterInstance]') AND type in (N'U'))
DROP TABLE [dbo].[PerfCounterInstance]
GO
/****** Object:  Table [dbo].[PerfCounterInstance]    Script Date: 01/01/2009 20:14:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerfCounterInstance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PerfCounterInstance](
    [CategoryName] [nvarchar](50) NOT NULL,
    [CounterName] [nvarchar](50) NOT NULL,
    [InstanceName] [nvarchar](50) NOT NULL,
    [Value] [int] NOT NULL,
 CONSTRAINT [PK_PerfCounterInstance] PRIMARY KEY CLUSTERED 
(
    [CategoryName] ASC,
    [CounterName] ASC,
    [InstanceName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

