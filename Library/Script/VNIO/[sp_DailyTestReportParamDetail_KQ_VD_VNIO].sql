/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.0.34
 * Time: 15/12/2011 9:00:16 PM
 ************************************************************/







alter PROCEDURE [dbo].[sp_DailyTestReportParamDetail_KQ_VD_VNIO](
    @pTestDateFrom  NVARCHAR(10)
   ,@pTestDateTo    NVARCHAR(10)
-- @pTestDateFrom DATETIME
--  ,@pTestDateTo    DATETIME
   ,@pTestType_ID   INT
   ,@ObjectType     SMALLINT
   ,@DepartMent_ID  INT
   ,@PID            NVARCHAR(50)
)
AS
	SELECT P.*
	      
	      ,ISNULL(
	           (
	               SELECT TOP 1 Test_result
	               FROM   T_RESULT_DETAIL
	               WHERE  TESTTYPE_ID = P.TESTTYPE_ID
	                      AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = P.TESTDATE
	                      AND Patient_ID = P.Patient_ID
	                      AND UPPER(Para_Name) = UPPER(P.Para_name)
	           )
	          ,'     '
	       ) Test_result
	      ,'' AS sStatus
	      ,'' AS sDate
	FROM   (
	           SELECT Patient_ID
	                 ,PATIENT_NAME
	                 ,PATIENT_Address
	                 ,Patient_InsuranceNum
	                 ,Patient_Age
	                 ,Patient_PID
	                 ,TEST_ID
	                 ,Patient_Sex
	                 ,PATIENT_sSex
	                 ,PATIENT_IDENTITY
	                 ,TESTDATE
	                 ,TESTTYPE_NAME
	                 ,Diagnostic,T1.Department
	                 ,T2.*
	           FROM   (
	                      SELECT Patient_ID
	                            ,TEST_ID
	                            ,T.TESTTYPE_ID
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
	                            ,(
	                                 SELECT Diagnostic
	                                 FROM   L_PATIENT_INFO
	                                 WHERE  PATIENT_ID = T.PATIENT_ID
	                             ) AS Diagnostic
	                            ,' ' AS PATIENT_sSex
	                            ,' ' AS PATIENT_IDENTITY
	                           ,CONVERT(NVARCHAR(10) ,TEST_DATE ,103) AS 
	                             ---,CONVERT(NVARCHAR(10) ,GETDATE() ,103) AS 
	                             TESTDATE
	                            ,(
	                                 SELECT TESTTYPE_NAME
	                                 FROM   T_TEST_TYPE_LIST
	                                 WHERE  TESTTYPE_ID = T.TESTTYPE_ID
	                             ) AS TESTTYPE_NAME,
	                             (SELECT TOP 1 V.khoa
	                                FROM tblHisLis_PatientInfo_VNIO V WHERE EXISTS(SELECT 1 FROM L_PATIENT_INFO lpi WHERE lpi.Patient_ID=T.Patient_ID AND V.maBenhNhan=lpi.PID))AS Department
	                      FROM   T_TEST_INFO T
	                      WHERE
--	                       dbo.trunc( TEST_DATE)BETWEEN dbo.trunc( @pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo)
--CONVERT(VARCHAR(8),GETDATE(),101)
	                       CONVERT(VARCHAR(10) ,TEST_DATE ,103) BETWEEN 
	                             CONVERT(VARCHAR(10) ,@pTestDateFrom ,103) AND 
	                             CONVERT(VARCHAR(10) ,@pTestDateTo ,103)
	                             AND (@pTestType_ID=-1 OR TESTTYPE_ID=@pTestType_ID)
	                             AND (
	                                     @PID='NOTHING'
	                                     OR (
	                                            SELECT PID
	                                            FROM   L_PATIENT_INFO
	                                            WHERE  PATIENT_ID = T.PATIENT_ID
	                                        )=@PID
	                             )
	                           AND EXISTS   (                                        
	                                            SELECT 1 
	                                            FROM   L_PATIENT_INFO
	                                            WHERE  PATIENT_ID = T.PATIENT_ID
	                                            AND EXISTS(SELECT 1 FROM tblHisLis_PatientInfo_VNIO P WHERE PID=P.maBenhNhan
	                                            AND (P.idKhoa IN(SELECT * FROM dbo.ConvertStringtoIntTable(@DepartMent_ID)
	                                             )
	                                            ) ))OR @DepartMent_ID=-1 
--	                             AND (
--	                                     @DepartMent_ID=-1
--	                                     OR (
--	                                            SELECT idkhoa
--	                                            FROM   tblHisLis_PatientInfo_VNIO TV
--	                                            WHERE  tv.maBenhNhan = 
--	                                        )=@DepartMent_ID
--	                                 )
	                             AND (
	                                     (@ObjectType=-1)
	                                     OR (
	                                            EXISTS (
	                                                SELECT 1
	                                                FROM   L_PATIENT_INFO
	                                                WHERE  PATIENT_ID = T.PATIENT_ID
	                                                       AND ObjectType = @ObjectType
	                                            )
	                                        )
	                             )
	                             AND T.Patient_ID>0
	                  )T1
	                 ,(
	                      SELECT T.*
	                      FROM   (
	                                 SELECT 'F' AS chon
	                                       ,DATACONTROL_ID
	                                       ,DATA_NAME AS Para_Name
	                                       ,ALIAS_NAME
	                                       ,DEVICE_ID
	                                       ,Data_Sequence
	                                       ,Measure_Unit
	                                       ,Normal_Level
	                                       ,Normal_LevelW
	                                       ,Data_Print
	                                       ,ISNULL(
	                                            (
	                                                SELECT TOP 1 TESTTYPE_ID
	                                                FROM   D_DEVICE_LIST
	                                                WHERE  DEVICE_ID = D.DEVICE_ID
	                                                       AND (@pTestType_ID =-3 OR TESTTYPE_ID =@pTestType_ID)
	                                            )
	                                           ,N''
	                                        ) AS TESTTYPE_ID
	                                 FROM   D_DATA_CONTROL D
	                             ) T
	                  )T2
	           WHERE  T1.TESTTYPE_ID = T2.TESTTYPE_ID
	       )
	       P
	       /*WHERE
	       EXISTS(SELECT 1 FROM T_REG_LIST
	       WHERE TEST_ID=P.TEST_ID
	       AND upper(P.Para_name)=upper(Para_name)
	       )*/
	ORDER BY
	       P.Patient_ID
	      ,P.TESTDATE
	      ,P.TestType_ID
	      ,Data_sequence
	      ,P.Para_Name
