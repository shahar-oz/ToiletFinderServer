Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'ToiletFinder_DB')
BEGIN
    DROP DATABASE ToiletFinder_DB;
END
Go
Create Database ToiletFinder_DB
Go
Use ToiletFinder_DB
Go

-- יצירת טבלת סוגי משתמשים
CREATE TABLE UTypes (
    UserType INT IDENTITY(1,1) PRIMARY KEY,                 -- מפתח ראשי
    TypeName NVARCHAR(100)                    -- שם של הסוג
)

-- יצירת טבלת משתמשים
CREATE TABLE Users (
    UserId INT PRIMARY KEY identity,           -- מפתח ראשי
    Username NVARCHAR(100),                    -- שם משתמש
    Pass NVARCHAR(100),                        -- סיסמא
    Email NVARCHAR(100) unique,                       -- מייל
    UserType INT                               -- מפתח משני, סוג משתמש 
      FOREIGN KEY (UserType) REFERENCES UTypes(UserType)   -- קישור לטבלת הסוגים
)

-- יצירת טבלת שירותים קיימים  
CREATE TABLE CurrentToilets (
    ToiletId INT PRIMARY KEY identity,        -- מפתח ראשי
    TLocation NVARCHAR(100),                  -- מיקום
    Accessibility BIT,                        -- נגישות
    Price FLOAT,                              -- מחיר
    --ToiletPictures VARBINARY(MAX)             -- תמונות
)

-- יצירת טבלת דירוגים
CREATE TABLE Rates (
    Rate INT,                                 -- הדירוג
    ToiletID INT PRIMARY KEY,                 -- מפתח משני וראשי מזהה שירותים
    FOREIGN KEY (ToiletId) REFERENCES CurrentToilets(ToiletId),  -- קישור לטבלת השירותים
)

-- יצירת טבלת ביקורות
CREATE TABLE Reviews (
    Review NVARCHAR(250),                           -- הביקורת
    ToiletID INT PRIMARY KEY,                  -- מפתח משני וראשי מזהה שירותים
    FOREIGN KEY (ToiletId) REFERENCES CurrentToilets(ToiletId),  -- קישור לטבלת השירותים
)

-- Create a login for the admin user
CREATE LOGIN [TaskAdminLogin] WITH PASSWORD = 'ShaharAdmin';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [TaskAdminUser] FOR LOGIN [TaskAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TaskAdminUser];
Go

INSERT INTO UTypes Values('General')
INSERT INTO UTypes Values('Service Provider')
INSERT INTO UTypes Values('Sanitaion Maneger')
INSERT INTO UTypes Values('Admin')

GO

INSERT INTO Users (Username, Pass, Email, UserType)  Values ('ShaharOz', 'ShaharOz1', 'shahar.oz@gmail.com',  4)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('ShaharShal', 'ShaharShal', 'shahar.shal@gmail.com',  1)
Go

INSERT INTO CurrentToilets Values('Baker Street 2, London', 0, 2)
Go
INSERT INTO Rates Values(3, 1)
Go
INSERT INTO Reviews Values('Fine. Not great.', 1)
Go
SELECT * FROM CurrentToilets
SELECT * FROM UTypes
SELECT * FROM Reviews
SELECT * FROM Rates
SELECT * FROM Users

--EF Code
/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=ToiletFinder_DB;User ID=TaskAdminLogin;Password=ShaharAdmin;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context ToiletDBContext -DataAnnotations -force
*/

