CREATE TABLE UserDetails
(
Id INT IDENTITY(1,1) PRIMARY KEY,
[Name] NVARCHAR(200),
MobileNumber NVARCHAR(10),
[Email] NVARCHAR(200),
[Address] NVARCHAR(250),
DateOfBirth DATETIME NULL,
Age INT,
CreatedBy INT,
ModifiedBy INT,
CreatedDate DATETIME NULL,
ModifiedDate DATETIME NULL,
IsActive BIT
)