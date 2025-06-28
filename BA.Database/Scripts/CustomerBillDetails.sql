CREATE TABLE CustomerBillDetails
(
Id INT IDENTITY(1,1) PRIMARY KEY,
CustomerName NVARCHAR(250),
CustomerAddress NVARCHAR(500),
NewsPaperIds NVARCHAR(100),
FromDate DATETIME NULL,
ToDate DATETIME NULL,
TotalDays INT,
Amount Decimal,
ServiceCharge Decimal,
TotalAmount Decimal,
CreatedDate DATETIME NULL,
ModifiedDate DATETIME NULL,
CreatedBy INT,
ModifiedBy INT,
IsBillPaid BIT
)