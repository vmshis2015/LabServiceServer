/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.0.34
 * Time: 25/11/2011 4:22:15 PM
 ************************************************************/

ALTER PROCEDURE [dbo].[spGetMaxBarcode]
(@pTestDate NVARCHAR(50))
AS
	SELECT MAX(RIGHT(barcode ,4))
	FROM   t_test_info
	WHERE  (LEFT(barcode+'000000' ,6) = @pTestDate) AND (ISNUMERIC(Barcode)=1)
