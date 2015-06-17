
alter PROCEDURE [dbo].[sp_GetTestInfoByPatientID]
	@patientId INT,
	@testTypeId NVARCHAR(40)
AS
	SELECT tti.Test_ID, tti.TestType_ID, tti.Barcode, tti.Test_Seq, tti.Patient_ID,
	       tti.Test_Date, tti.Assign_ID, tti.Require_Date, tti.Diagnostician_ID,
	       tti.Receiver_ID, tti.Test_Status, tti.Diag_Result, tti.Update_User,
	       tti.Update_Date, tti.His_Assign_id, tti.Para_ID, tti.Device_ID,
	       ISNULL (tti.Printstatus,0) AS Printstatus
	      ,1as CHON
	      ,0 AS IsNew
	      ,(
	           SELECT TOP 1 lts.TestStatus_Name
	           FROM   L_Test_Status lts
	           WHERE  lts.TestStatus_ID = tti.Test_Status
	       ) AS TestStatus_Name
	      ,(
	           SELECT TOP 1 tttl.TestType_Name
	           FROM   T_TEST_TYPE_LIST tttl
	           WHERE  tttl.TestType_ID = tti.TestType_ID
	       ) AS TestType_Name
	      ,(
	           SELECT TOP 1 ddl.Device_Name
	           FROM   D_DEVICE_LIST ddl
	           WHERE  ddl.Device_ID = tti.Device_ID
	       ) AS Device_Name
	       
	FROM   T_TEST_INFO tti
	WHERE  tti.Patient_ID = @patientId AND tti.TestType_ID IN (SELECT *
	                                      FROM   dbo.ConvertStringtoIntTable(@testTypeId))
	--(tti.TestType_ID=@testTypeId) --OR (@testTypeId=-100))
	
	SELECT 1 AS CHON ,*
	      ,0 AS IsNew
	      ,ROW_NUMBER() OVER(ORDER BY trd.TestDetail_ID DESC) AS STT
	FROM   T_RESULT_DETAIL trd
	WHERE  trd.Patient_ID = @patientId
	       AND EXISTS(
	               SELECT 1
	               FROM   T_TEST_INFO tti
	               WHERE  tti.Patient_ID = trd.Patient_ID
	           )
	ORDER BY
	       trd.Data_Sequence asc

