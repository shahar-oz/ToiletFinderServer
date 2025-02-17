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
StatusDesc NVARCHAR (50) Not Null
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
    StatusID INT,                                      -- מפתח משני, סטטוס
FOREIGN KEY (StatusID) REFERENCES Statuses(StatusID),
  UserId INT,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)

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

INSERT INTO UTypes Values( 'General')
INSERT INTO UTypes Values( 'Service Provider')
INSERT INTO UTypes Values( 'Admin')

Go

INSERT INTO Statuses Values(1, 'Approved')
INSERT INTO Statuses Values(2, 'Pending')
INSERT INTO Statuses Values(3, 'Declined')


GO

INSERT INTO Users (Username, Pass, Email, UserType)  Values ('Admin', 'Admin123', 'admin@gmail.com',  3)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('1', '1', '1',  3)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('SP', 'SP', 'SP',  2)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('SP2', 'SP2', 'SP2',  2)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('SP3', 'SP3', 'SP3',  2)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('User1', 'User1', 'User1',  1)
INSERT INTO Users (Username, Pass, Email, UserType)  Values ('ShaharShal', 'ShaharShal', 'shahar.shal@gmail.com',  1)

Go

INSERT INTO CurrentToilets Values('Baker Street 2, London', 0, 2, 1,5)
INSERT INTO CurrentToilets Values('Ramataim 21, Hod Hasharon', 0, 2, 2,3)
INSERT INTO CurrentToilets Values('Ahuza 14, Rannana', 0, 2, 2,4)
INSERT INTO CurrentToilets Values('Habanim 12, Hod Hasharon', 0, 2, 2,3)
INSERT INTO CurrentToilets Values('Gaza 23, Jerusalem', 1, 2, 1,5)
INSERT INTO CurrentToilets Values('Jaffa 14, Tel Aviv', 0, 2, 1,4)
INSERT INTO CurrentToilets Values('Namir 133, Jerusalem', 0, 2, 1,5)
INSERT INTO CurrentToilets Values('Begin 81, Tel Aviv', 1, 1.5, 2,4)
INSERT INTO CurrentToilets Values('Shamir 123, Jerusalem', 0, 2, 1,5)
INSERT INTO CurrentToilets Values('Hamered 13, Tel Aviv', 0, 0, 2,4)
INSERT INTO CurrentToilets Values('Balfur 23, Jerusalem', 0, 4, 1,5)
INSERT INTO CurrentToilets Values('Hamacabim 14, Modiin', 0, 0, 3,3)
INSERT INTO CurrentToilets Values('Ganot 12, Rishon', 1, 1, 3,4)
INSERT INTO CurrentToilets Values('Navon 2, Yavne', 0, 2, 3,5)

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

