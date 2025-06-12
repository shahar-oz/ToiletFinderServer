
USE master;
GO

-- Declare the database name
DECLARE @DatabaseName NVARCHAR(255) = 'ToiletFinder_DB';

-- Generate and execute the kill commands for all active connections
DECLARE @KillCommand NVARCHAR(MAX);

SET @KillCommand = (
    SELECT STRING_AGG('KILL ' + CAST(session_id AS NVARCHAR), '; ')
    FROM sys.dm_exec_sessions
    WHERE database_id = DB_ID(@DatabaseName)
);

IF @KillCommand IS NOT NULL
BEGIN
    EXEC sp_executesql @KillCommand;
    PRINT 'All connections to the database have been terminated.';
END
ELSE
BEGIN
    PRINT 'No active connections to the database.';
END
Go

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'ToiletFinder_DB')
BEGIN
    DROP DATABASE ToiletFinder_DB;
END
Go

CREATE Database ToiletFinder_DB;
Go


drop login [TaskAdminLogin]
-- Create a login for the admin user
CREATE LOGIN [TaskAdminLogin] WITH PASSWORD = 'ShaharAdmin';
Go

ALTER LOGIN [TaskAdminLogin] WITH PASSWORD = 'ShaharAdmin';
Go

Use ToiletFinder_DB
Go
drop user [TaskAdminUser]
-- Create a user in the TamiDB database for the login
CREATE USER [TaskAdminUser] FOR LOGIN [TaskAdminLogin];
Go


-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TaskAdminUser];
Go

Use master
Go


               USE master;
               DECLARE @latestBackupSet INT;
               SELECT TOP 1 @latestBackupSet = position
               FROM msdb.dbo.backupset
               WHERE database_name = 'ToiletFinder_DB'
               AND backup_set_id IN (
                     SELECT backup_set_id
                     FROM msdb.dbo.backupmediafamily
                     WHERE physical_device_name = 'C:\Users\User\source\repos\shahar-oz\ToiletFinderServer\ToiletFinderServer\wwwroot\..\DBScripts\backup.bak'
                 )
               ORDER BY backup_start_date DESC;
                ALTER DATABASE ToiletFinder_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE ToiletFinder_DB FROM DISK = 'C:\Users\User\source\repos\shahar-oz\ToiletFinderServer\ToiletFinderServer\wwwroot\..\DBScripts\backup.bak' 
                WITH REPLACE;
                ALTER DATABASE ToiletFinder_DB SET MULTI_USER;

                select * from CurrentToilets