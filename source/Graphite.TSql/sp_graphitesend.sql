USE [master]
GO

-- enable 'TRUSTWORTHY'
-- required for 'external access' of clr code (-> sending tcp packets).
ALTER DATABASE [master] SET TRUSTWORTHY ON;
GO

-- enable clr code
EXEC sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO

-- Check if procedure and assembly already exist and drop them
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_graphitesend]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_graphitesend]
GO
IF  EXISTS (SELECT * FROM sys.assemblies WHERE name = N'Graphite.TSql')
DROP ASSEMBLY [Graphite.TSql]
GO

-- Add Graphite.TSql.dll assembly
CREATE ASSEMBLY [Graphite.TSql]
AUTHORIZATION [dbo]
FROM '<your/path/to/Graphite.TSql.dll>'
WITH PERMISSION_SET = EXTERNAL_ACCESS
GO

-- Create stored procedure 'sp_graphitesend'
CREATE PROCEDURE sp_graphitesend
(
@host nvarchar(255),
@port int,
@key nvarchar(255),
@value int
)
AS
EXTERNAL NAME [Graphite.TSql].[Graphite.TSql.GraphiteProcedures].GraphiteSend
GO
-- --------------------------------------------------------------------------
-- Example usage:
--
-- EXEC sp_graphitesend N'192.168.0.1', 2003, 'stats.events.myserver.test', 1
--
-- --------------------------------------------------------------------------
GO

-- Create stored procedure 'sp_graphitesendseries'
CREATE PROCEDURE sp_graphitesendseries
(
@host	nvarchar(255),
@port	int,
@points	 NVARCHAR(MAX),
@returnstring	 NVARCHAR(MAX) OUTPUT
)
AS
EXTERNAL NAME [Graphite.TSql].[Graphite.TSql.GraphiteProcedures].GraphiteSendSeries
GO
-- --------------------------------------------------------------------------
-- Example usage:
-- Send several series points to graphite to optimize clr by using xml syntax.
-- CREATE TABLE #GraphiteTable ([key] nvarchar(255), value INT );  
--
-- INSERT INTO #GraphiteTable VALUES ('stats.events.myserver.test', 15);
-- INSERT INTO #GraphiteTable VALUES ('stats.events.myserver.test1', 12);
-- INSERT INTO #GraphiteTable VALUES ('stats.events.myserver.test2', 87);
-- 
-- DECLARE @outputString NVARCHAR(MAX) 
-- DECLARE @inputString NVARCHAR(MAX) 
-- SET @inputString = (SELECT * FROM #GraphiteTable FOR XML PATH('Points'), ROOT('Root'))   
-- EXEC sp_graphitesendseries N'192.168.0.1', 2003, @inputString, @outputString OUT
-- SELECT @outputString
--
-- --------------------------------------------------------------------------
GO
