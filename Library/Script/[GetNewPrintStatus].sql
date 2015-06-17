


/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.0.70
 * Time: 16/01/2012 10:54:14 AM
 ************************************************************/

CREATE FUNCTION [dbo].[GetNewPrintStatus]
(
	@patientId INT
	,	@pTestDateFrom NVARCHAR(10)
	,@pTestDateTo NVARCHAR(10)
	
)
RETURNS nvarchar(10)
AS
 
BEGIN
	DECLARE @result nvarchar(10)
	DECLARE @totalTest INT
	DECLARE @printedTest INT
	set @result='';
	SELECT @totalTest = COUNT(tti.Test_ID)
	FROM   T_TEST_INFO tti
	WHERE  tti.Patient_ID = @patientId 
	 and (  CONVERT(DATETIME, CONVERT(NVARCHAR(10), tti.TEST_DATE, 103), 103) 
	           BETWEEN CONVERT(DATETIME, @pTestDateFrom, 103)
	           
	           AND
	           CONVERT(DATETIME, @pTestDateTo, 103)
	           )
	SELECT @printedTest = COUNT(tti.Test_ID)
	FROM   T_TEST_INFO tti
	WHERE  (tti.Patient_ID = @patientId)
	       AND (tti.Printstatus = 1)-- AND (tti.TEST_DATE=@datetime)
	     and (  CONVERT(DATETIME, CONVERT(NVARCHAR(10), tti.TEST_DATE, 103), 103) 
	           BETWEEN CONVERT(DATETIME, @pTestDateFrom, 103)
	           
	           AND
	           CONVERT(DATETIME, @pTestDateTo, 103)
	           )
	DECLARE @TestType_ID int
	DECLARE _CURSOR CURSOR FOR
    SELECT TestType_ID
	FROM   T_TEST_INFO tti
	WHERE  (tti.Patient_ID = @patientId)
	       AND (tti.Printstatus = 1)-- AND (tti.TEST_DATE=@datetime)
	     and (  CONVERT(DATETIME, CONVERT(NVARCHAR(10), tti.TEST_DATE, 103), 103) 
	           BETWEEN CONVERT(DATETIME, @pTestDateFrom, 103)
	           
	           AND
	           CONVERT(DATETIME, @pTestDateTo, 103)
	           )
	order by TestType_ID  
	IF @printedTest = 0
	    SET @result = '0'
	ELSE
		BEGIN
			OPEN _CURSOR

				-- Perform the first fetch.
				FETCH NEXT FROM _CURSOR
				INTO @TestType_ID
				-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
				WHILE @@FETCH_STATUS = 0
					BEGIN
						SET @result = @result + convert(nvarchar(10),@TestType_ID)
						-- This is executed as long as the previous fetch succeeds.
						FETCH NEXT FROM _CURSOR
						INTO @TestType_ID
					END

			CLOSE _CURSOR
			DEALLOCATE _CURSOR

		END
	    
	  -- SET @result =@result+convert(nvarchar(10),45)
	    RETURN @result
END


