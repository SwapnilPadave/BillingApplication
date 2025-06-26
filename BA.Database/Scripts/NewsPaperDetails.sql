CREATE TABLE NewsPaperDetails
(
Id INT IDENTITY(1,1) PRIMARY KEY,
[Name] NVARCHAR(100),
[Language] NVARCHAR(100),
CreatedBy INT,
ModifiedBy INT,
CreatedDate DATETIME NULL,
ModifiedDate DATETIME NULL,
IsActive BIT
)