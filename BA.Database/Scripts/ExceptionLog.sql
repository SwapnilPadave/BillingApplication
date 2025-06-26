CREATE TABLE ExceptionLog
(
Id INT IDENTITY(1,1) PRIMARY KEY,
CreatedDate DATETIME NULL,
ExceptionMessage NVARCHAR(MAX),
ExceptionType NVARCHAR(MAX),
StackTrace NVARCHAR(MAX),
InnerExceptionMessage NVARCHAR(MAX)
)