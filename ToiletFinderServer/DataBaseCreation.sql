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

--יצירת טבלת סטטוס
CREATE TABLE Statuses(
StatusID INT PRIMARY KEY,
StatusDesc NVARCHAR (50)
);


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
    StatusID INT,                                      -- מפתח משני, סטטוס
FOREIGN KEY (StatusID) REFERENCES Statuses(StatusID)

)

-- יצירת טבלת תמונות של שירותים קיימים  
CREATE TABLE CurrentToiletsPhotos (
    PhotoId INT PRIMARY KEY identity,        -- מפתח ראשי
    ToiletId INT FOREIGN KEY REFERENCES CurrentToilets(ToiletId),        -- מפתח משני
    

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

Go

INSERT INTO Statuses Values('Approved')
INSERT INTO Statuses Values('Pending')
INSERT INTO Statuses Values('Declined')


GO

INSERT INTO Users (Username, Pass, Email, UserType)  Values ('Admin', 'Admin123', 'admin@gmail.com',  4)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('Service', 'Service123', 'service@gmail.com',  2)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('ShaharShal', 'ShaharShal', 'shahar.shal@gmail.com',  1)
Go

INSERT INTO CurrentToilets Values('Baker Street 2, London', 0, 2, 1)
INSERT INTO CurrentToilets Values('Ramataim 21, Hod Hasharon', 0, 2, 2)
INSERT INTO CurrentToilets Values('Ahuza 14, Rannana', 0, 2, 2)
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
select * from CurrentToiletsPhotos

--EF Code
/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=ToiletFinder_DB;User ID=TaskAdminLogin;Password=ShaharAdmin;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context ToiletDBContext -DataAnnotations -force
*/

