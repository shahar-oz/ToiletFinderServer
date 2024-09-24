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
    UserType INT PRIMARY KEY,                 -- מפתח ראשי
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
    UserType INT,                              -- מפתח משני, סוג משתמש 
      FOREIGN KEY (UserType) REFERENCES UTypes(UserType)   -- קישור לטבלת הסוגים
)

-- יצירת טבלת שירותים קיימים  
CREATE TABLE CurrentToilets (
    ToiletId INT PRIMARY KEY identity,        -- מפתח ראשי
    TLocation NVARCHAR(100),                  -- שם משתמש
    Accessibility NVARCHAR(100),              -- סיסמא
    Price FLOAT,                              -- מייל
    ToiletPictures VARBINARY(MAX)             -- תמונות
)

-- יצירת טבלת דירוגים
CREATE TABLE Rates (
    Rate INT,                                 -- הדירוג
    ToiletID INT PRIMARY KEY,                 -- מפתח משני וראשי מזהה שירותים
    FOREIGN KEY (ToiletId) REFERENCES CurrentToilets(ToiletId),  -- קישור לטבלת השירותים
)

-- יצירת טבלת ביקורות
CREATE TABLE Reviews (
    Review NVARCHAR,                           -- הביקורת
    ToiletID INT PRIMARY KEY,                  -- מפתח משני וראשי מזהה שירותים
    FOREIGN KEY (ToiletId) REFERENCES CurrentToilets(ToiletId),  -- קישור לטבלת השירותים
)
