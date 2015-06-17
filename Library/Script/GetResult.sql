set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








alter PROCEDURE GetResult
(
@TestType_ID int,
@pTestDate DateTime
)
AS
SELECT *
--SELECT DT.Patient_ID,DT.Test_ID,DT.Test_Date,DT.Test_Result,DT.Para_Name,DT.Normal_levelW,DT.Normal_Level,DT.Measure_Unit
FROM T_RESULT_DETAIL DT
WHERE 
convert(nvarchar(15),TEST_DATE,103)=convert(nvarchar(15),@pTestDate,103)

AND TestType_ID=@TestType_ID
SELECT TEST_ID
FROM T_TEST_INFO
WHERE convert(nvarchar(15),TEST_DATE,103)=convert(nvarchar(15),@pTestDate,103)

AND testtype_ID=@TestType_ID
