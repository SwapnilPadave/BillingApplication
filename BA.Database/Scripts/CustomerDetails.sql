CREATE TABLE CustomerDetails
(
Id INT IDENTITY(1,1) PRIMARY KEY,
BuildingName NVARCHAR(50),
RoomNo NVARCHAR(20),
AreaName NVARCHAR(50),
CreatedBy INT,
ModifiedBy INT,
CreatedDate DATETIME NULL,
ModifiedDate DATETIME NULL,
IsActive BIT
)