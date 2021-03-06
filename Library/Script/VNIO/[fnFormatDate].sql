set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER FUNCTION [dbo].[fnFormatDate] (
	@Datetime DATETIME
	
)

RETURNS VARCHAR(10)

AS

BEGIN

    DECLARE @StringDate VARCHAR(10)

    SET @StringDate = @Datetime

    IF (CHARINDEX ('YYYY',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'YYYY',

                         DATENAME(YY, @Datetime))

    IF (CHARINDEX ('YY',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'YY',

                         RIGHT(DATENAME(YY, @Datetime),2))

    IF (CHARINDEX ('Month',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'Month',

                         DATENAME(MM, @Datetime))

    IF (CHARINDEX ('MON',@StringDate COLLATE SQL_Latin1_General_CP1_CS_AS)>0)

       SET @StringDate = REPLACE(@StringDate, 'MON',

                         LEFT(UPPER(DATENAME(MM, @Datetime)),3))

    IF (CHARINDEX ('Mon',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'Mon',

                                     LEFT(DATENAME(MM, @Datetime),3))

    IF (CHARINDEX ('MM',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'MM',

                  RIGHT('0'+CONVERT(VARCHAR,DATEPART(MM, @Datetime)),2))

    IF (CHARINDEX ('M',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'M',

                         CONVERT(VARCHAR,DATEPART(MM, @Datetime)))

    IF (CHARINDEX ('DD',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'DD',

                         RIGHT('0'+DATENAME(DD, @Datetime),2))

    IF (CHARINDEX ('D',@StringDate) > 0)

       SET @StringDate = REPLACE(@StringDate, 'D',

                                     DATENAME(DD, @Datetime))   

RETURN @StringDate

END

--SELECT dbo.fnFormatDate (getdate(), '2011-12-09 00:00:00.000') 
