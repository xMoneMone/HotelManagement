CREATE DATABASE HotelManagement;
USE HotelManagement;

CREATE TABLE Colors
(
Id INT PRIMARY KEY IDENTITY,
Color VARCHAR(6) NOT NULL
);

CREATE TABLE AccountTypes
(
Id INT PRIMARY KEY IDENTITY,
[Type] VARCHAR(100)
);

CREATE TABLE Users
(
Id INT PRIMARY KEY IDENTITY,
ColorId INT FOREIGN KEY REFERENCES Colors(Id) NOT NULL,
AccountTypeId INT FOREIGN KEY REFERENCES AccountTypes(Id),
Email VARCHAR(320) NOT NULL,
[Password] VARCHAR(300) NOT NULL,
FirstName VARCHAR(200) NOT NULL,
LastName VARCHAR(200) NOT NULL,
);

CREATE TABLE Currencies
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(300) NOT NULL,
FormattingString VARCHAR(300) NOT NULL
);

CREATE TABLE Hotels
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(300) NOT NULL,
CurrencyId INT FOREIGN KEY REFERENCES Currencies(Id),
DownPaymentPercentage INT,
);

CREATE TABLE HotelCodes
(
Code VARCHAR(200) NOT NULL,
HotelId INT FOREIGN KEY REFERENCES Hotels(Id)
);

CREATE TABLE UsersHotels
(
UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE NOT NULL,
HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL
);

CREATE TABLE Rooms
(
Id INT PRIMARY KEY IDENTITY,
RoomNumber VARCHAR(200) NOT NULL,
PricePerNight MONEY NOT NULL,
Notes VARCHAR(300),
HotelId INT FOREIGN KEY REFERENCES Hotels(Id) ON DELETE CASCADE NOT NULL 
);

CREATE TABLE Beds
(
Id INT PRIMARY KEY IDENTITY,
BedType VARCHAR(200) NOT NULL,
Capacity INT NOT NULL
);

CREATE TABLE RoomsBeds
(
RoomId INT FOREIGN KEY REFERENCES Rooms(Id) ON DELETE CASCADE NOT NULL,
BedId INT FOREIGN KEY REFERENCES Beds(Id) NOT NULL
);

CREATE TABLE Bookings
(
Id INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(200) NOT NULL,
LastName VARCHAR(200) NOT NULL,
StartDate DATETIME NOT NULL,
EndDate DATETIME NOT NULL,
DownPaymentPaid BIT NOT NULL,
FullPaymentPaid BIT NOT NULL,
DownPaymentPrice Money NOT NULL,
FullPaymentPrice Money NOT NULL,
Notes VARCHAR(300),
RoomId INT FOREIGN KEY REFERENCES Rooms(Id) ON DELETE CASCADE NOT NULL
);

CREATE TABLE Extras
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(300) NOT NULL,
Price MONEY NOT NULL,
HotelId INT FOREIGN KEY REFERENCES Hotels(Id) ON DELETE CASCADE NOT NULL
);

CREATE TABLE BookingsExtras
(
ExtraId INT FOREIGN KEY REFERENCES Extras(Id) ON DELETE CASCADE NOT NULL,
BookingId INT FOREIGN KEY REFERENCES Bookings(Id) NOT NULL
);