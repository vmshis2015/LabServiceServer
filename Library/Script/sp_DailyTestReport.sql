/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.0.34
 * Time: 15/12/2011 8:53:21 PM
 ************************************************************/








ALTER PROCEDURE sp_DailyTestReport(
    @pTestDateFrom  DATETIME
   ,@pTestDateTo    DATETIME
   ,@pTestType_ID   INT
)
AS
	SELECT Patient_ID
	      ,(
	           SELECT PATIENT_NAME
	           FROM   L_PATIENT_INFO
	           WHERE  PATIENT_ID = T.PATIENT_ID
	       ) AS PATIENT_NAME
	      ,(
	           SELECT ADDRESS
	           FROM   L_PATIENT_INFO
	           WHERE  PATIENT_ID = T.PATIENT_ID
	       ) AS PATIENT_Address
	      ,(
	           SELECT Insurance_Num
	           FROM   L_PATIENT_INFO
	           WHERE  PATIENT_ID = T.PATIENT_ID
	       ) AS Patient_InsuranceNum
	      ,(
	           SELECT Age
	           FROM   L_PATIENT_INFO
	           WHERE  PATIENT_ID = T.PATIENT_ID
	       ) AS Patient_Age
	      ,(
	           SELECT PID
	           FROM   L_PATIENT_INFO
	           WHERE  PATIENT_ID = T.PATIENT_ID
	       ) AS Patient_PID
	      ,(
	           SELECT sex
	           FROM   L_PATIENT_INFO
	           WHERE  PATIENT_ID = T.PATIENT_ID
	       ) AS Patient_Sex
	      ,' ' AS PATIENT_sSex
	      ,' ' AS PATIENT_IDENTITY
	      ,CONVERT(NVARCHAR(10) ,TEST_DATE ,103) AS TESTDATE
	      ,(
	           SELECT TESTTYPE_NAME
	           FROM   T_TEST_TYPE_LIST
	           WHERE  TESTTYPE_ID = T.TESTTYPE_ID
	       ) AS TESTTYPE_NAME
	      ,ISNULL(
	           (
	               SELECT MAX(testtype_ID)
	               FROM   T_RESULT_DETAIL
	               WHERE  TESTTYPE_ID = T.TESTTYPE_ID
	             AND   dbo.trunc( TEST_DATE)BETWEEN dbo.trunc( @pTestDateFrom) 
	                  AND dbo.trunc (@pTestDateTo)
	                     -- AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = CONVERT(NVARCHAR(10) ,T.TEST_DATE ,103)
	                      AND PATIENT_ID = T.PATIENT_ID
	           )
	          ,-1
	       ) AS STATUS
	      ,' ' AS sStatus
	      ,'' AS sDate
	FROM   T_TEST_INFO T
	WHERE 
	dbo.trunc( TEST_DATE)BETWEEN dbo.trunc( @pTestDateFrom) 
	                  AND dbo.trunc (@pTestDateTo)
--	 CONVERT(DATETIME ,TEST_DATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103) 
--	       AND CONVERT(DATETIME ,@pTestDateTo ,103)
--	       
	       AND (@pTestType_ID=-1 OR TESTTYPE_ID=@pTestType_ID)






