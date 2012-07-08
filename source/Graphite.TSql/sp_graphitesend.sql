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
-- exec sp_graphitesend N'192.168.0.1', 2003, 'stats.events.myserver.test', 1
--
-- --------------------------------------------------------------------------