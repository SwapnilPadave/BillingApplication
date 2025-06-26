CREATE PROCEDURE USP_ExceptionLogToDatabase
@Date DATETIME,
@Message NVARCHAR(MAX) NULL,
@Type NVARCHAR(MAX) NULL,
@StackTrace NVARCHAR(MAX) NULL,
@InnerException NVARCHAR(MAX) NULL

AS
BEGIN
	INSERT INTO ExceptionLog
	(
	CreatedDate,
	ExceptionMessage,
	ExceptionType,
	StackTrace,
	InnerExceptionMessage
	)
	SELECT
	@Date,
	@Message,
	@Type,
	@StackTrace,
	@InnerException
END