
/*******************************************************************************************/
--
--Stored procedure RegisterCategory() is not provided because it is hard to abstract
--
/*******************************************************************************************/



/*******************************************************************************************/
-- Create stored prcodure UnregisterPerfCategory.
-- This sproc is delete a perf counter category
/*******************************************************************************************/

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'sp_PerfCounterUnregisterCategory')
    BEGIN
        DROP  Procedure  sp_PerfCounterUnregisterCategory
    END

GO

CREATE Procedure sp_PerfCounterUnregisterCategory
    (
        @categoryName nvarchar(50)
    )

AS

BEGIN

   DELETE FROM PerfCounterCategory
   WHERE [CategoryName] = @categoryName

END

GO


/*******************************************************************************************/
-- Create stored prcodure UnregisterPerfCategory.
-- This sproc is delete a perf counter category
/*******************************************************************************************/

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'sp_PerfCounterCreateInstance')
    BEGIN
        DROP  Procedure  sp_PerfCounterCreateInstance
    END

GO

CREATE Procedure sp_PerfCounterCreateInstance
    (
        @categoryName nvarchar(50),
        @instanceName nvarchar(50)
    )

AS

BEGIN

-- Clear the  already existing performance instance
  DELETE FROM PerfCounterInstance
  WHERE [CategoryName] = @categoryName AND [InstanceName] = @instanceName

-- Create instance according to performance category information
  INSERT PerfCounterInstance
  SELECT [CategoryName], [CounterName], @instanceName, 0 FROM PerfCounterCategory WHERE [CategoryName] = @categoryName


END

GO


/*******************************************************************************************/
-- Create stored prcodure PerfCounterSetValue.
-- This sproc is used to update the specified counter to specific value
/*******************************************************************************************/

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'sp_PerfCounterSetValue')
    BEGIN
        DROP  Procedure  sp_PerfCounterSetValue
    END

GO

CREATE Procedure sp_PerfCounterSetValue
    (
        @categoryName nvarchar(50),
        @counterName nvarchar(50),
        @instanceName nvarchar(50),
        @value int
    )

AS

BEGIN

  UPDATE PerfCounterInstance SET [Value]=@value
  WHERE [CategoryName] = @categoryName AND
    [CounterName] = @counterName AND
    [InstanceName] = @instanceName

END

GO

/*******************************************************************************************/
-- Create stored prcodure PerfCounterIncreateBy.
-- This sproc is used to increment the specified counter by specific value
/*******************************************************************************************/

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'sp_PerfCounterIncrementBy')
    BEGIN
        DROP  Procedure  sp_PerfCounterIncrementBy
    END

GO

CREATE Procedure sp_PerfCounterIncrementBy
    (
        @categoryName nvarchar(50),
        @counterName nvarchar(50),
        @instanceName nvarchar(50),
        @value int
    )

AS

BEGIN

  UPDATE PerfCounterInstance SET [Value]= [Value] + @value
  WHERE [CategoryName] = @categoryName AND
    [CounterName] = @counterName AND
    [InstanceName] = @instanceName

END

GO


