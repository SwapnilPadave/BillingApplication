ALTER PROCEDURE USP_GetUserLoginDetails
@UserId NVARCHAR(100),
@Password NVARCHAR(100)

AS
BEGIN
	SELECT
	ud.Id AS UserId,
	l.UserName AS UserName,
	ud.MobileNumber AS MobileNumber,
	ud.Email AS EmailAddress,
	ud.IsActive AS IsActive,
	l.IsAdmin AS [Admin]
	FROM [Login] l
	JOIN UserDetails ud ON l.UserId = ud.Id
	WHERE 
	l.UserId = @UserId AND 
	l.[Password] = @Password AND
	ud.IsActive = 1
END