﻿Use master
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
    Email NVARCHAR(100),                       -- מייל
    PhoneNumber NVARCHAR(100),                 -- מספר טלפון
    DateOfBirth DATE ,                         -- תאריך לידה של המשתמש
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

INSERT INTO UTypes Values('General')
INSERT INTO UTypes Values('Service Provider')
INSERT INTO UTypes Values('Sanitaion Maneger')
INSERT INTO UTypes Values('Admin')

INSERT INTO Users Values('ShaharOz', 'ShaharOz1', 'shahar.oz@gmail.com', '054-5264648', '2007/08/29', 4)
INSERT INTO Users Values('ShaharShalgi', 'Fluffy234', 'shahar.shalgi@gmail.com', '052-5381648', '2007/02/26', 1)

INSERT INTO CurrentToilets Values('Baker Street 2, London', 0, 2)

INSERT INTO Rates Values(3, 1)

INSERT INTO Reviews Values('Fine. Not great.', 1)

SELECT * FROM CurrentToilets



