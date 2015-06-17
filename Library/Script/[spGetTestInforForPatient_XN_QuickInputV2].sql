
/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.0.34
 * Time: 20/12/2011 9:47:44 AM
 ************************************************************/



alter PROCEDURE [dbo].[spGetTestInforForPatient_XN_QuickInputV2]
(
    @pTestDate     NVARCHAR(10)
   ,@TestTypeID    INT
   ,@PID           NVARCHAR(50)
   ,@Patient_Name  NVARCHAR(50)
   ,@Age           INT
   ,@Sex           INT
   ,@HasTest       INT
   ,@pcheck bit
)
AS
IF (@pcheck='TRUE')
	BEGIN
		
	SELECT P1.*
	FROM   (
	           SELECT P.*
	                 ,0 AS STT
	                 ,(
	                      SELECT TOP 1 Barcode
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  ) AS Barcode
	                
	                 ,(
	                      SELECT TOP 1 Test_ID
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  ) AS Test_ID
	                  ,(
	                      SELECT TOP 1 TestType_ID
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  ) AS TestType_ID
	                   ,(
	           SELECT TOP 1 dbo.PatientSex(lpi.Sex)
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = p.Patient_ID
	       ) AS Sex_Name
	           FROM   L_PATIENT_INFO P left JOIN tblYeucauXetnghiem_VNIO t2 ON t2.maBenhNhan=p.PID
	           WHERE   t2.maBenhNhan=P.PID
	              	and   (t2.idCanLamSangThucHien = 47 )
			
	            AND EXISTS(
	                      SELECT *
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  )
	                  AND (
	                          (@HasTest=1)
	                          OR (
	                                 @HasTest=0
	                                 AND (
	                                         EXISTS(
	                                             SELECT 1
	                                             FROM   D_DATA_CONTROL
	                                             WHERE  DEvice_ID IN (SELECT 
	                                                                         device_ID
	                                                                  FROM   
	                                                                         D_Device_List 
	                                                                         D
	                                                                  WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID))
	                                                    AND UPPER(Alias_Name)NOT IN (SELECT 
	                                                                                        UPPER(Para_name)
	                                                                                 FROM   
	                                                                                        T_RESULT_DETAIL 
	                                                                                        T
	                                                                                 WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID)
	                                                                                        AND 
	                                                                                            Patient_ID = 
	                                                                                            P.PAtient_ID
	                                                                                        AND 
	                                                                                            CONVERT(NVARCHAR(10) , TEST_DATE , 103) = 
	                                                                                            @pTestDate
	                                                                                        AND 
	                                                                                            PARA_STATUS = 
	                                                                                            1)
	                                         )
	                                         AND EXISTS--kiem tra dangky XN 2 chieu
	                                             (
	                                                 SELECT *
	                                                 FROM   T_REG_LIST
	                                                 WHERE  TEST_ID IN (SELECT 
	                                                                           TEST_ID
	                                                                    FROM   
	                                                                           T_TEST_INFO
	                                                                    WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID)
	                                                                           AND 
	                                                                               Patient_ID = 
	                                                                               P.PAtient_ID
	                                                                           AND 
	                                                                               CONVERT(NVARCHAR(10) , TEST_DATE , 103) = 
	                                                                               @pTestDate)
	                                                        AND UPPER(Para_name) 
	                                                            NOT IN (SELECT 
	                                                                           UPPER(Para_name)
	                                                                    FROM   
	                                                                           T_RESULT_DETAIL 
	                                                                           T
	                                                                    WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID)
	                                                                           AND 
	                                                                               Patient_ID = 
	                                                                               P.PAtient_ID
	                                                                           AND 
	                                                                               CONVERT(NVARCHAR(10) , TEST_DATE , 103) = 
	                                                                               @pTestDate
	                                                                           AND 
	                                                                               PARA_STATUS = 
	                                                                               1)
	                                             )--end Or
	                                     )--end exists
	                             )--end OR (@HasTest=0
	                      )
	                  AND (@PID='NOTHING' OR PID=@PID)
	                  AND (
	                          @Patient_Name='NOTHING'
	                          OR Patient_Name LIKE '%'+@Patient_Name+'%'
	                      )
	                  AND (@Age=0 OR Age=@Age)
	                  AND (@Sex=3 OR Sex=@Sex)
	       )P1
	       
	      
	ORDER BY
	       P1.Barcode ASC
	END
	
	ELSE
		SELECT P1.*
	FROM   (
	           SELECT P.*
	                 ,0 AS STT
	                 ,(
	                      SELECT TOP 1 Barcode
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  ) AS Barcode
	                   ,(
	                      SELECT TOP 1 Lis_ParaName
	                      FROM  tbl_ParamMapping tpm left join tblYeucauXetnghiem_VNIO t ON t.idCanLamSangThucHien = tpm.Med_ParamID
	                      WHERE  (@TestTypeID=-3 OR t.TestType_ID=@TestTypeID)
	                            AND t.maBenhNhan =p.PID 
	                            or  (t.idCanLamSangThucHien=46 or  t.idCanLamSangThucHien=47)
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                             and  EXISTS(
	                      SELECT *
	                      FROM   tblYeucauXetnghiem_VNIO t1
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                        and  (t1.idCanLamSangThucHien=46 or  t1.idCanLamSangThucHien=47)
	                              AND t1.maBenhNhan =p.PID 
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  )
	                  ) AS MCMD
	                 ,(
	                      SELECT TOP 1 Test_ID
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  ) AS Test_ID
	                  ,(
	                      SELECT TOP 1 TestType_ID
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  ) AS TestType_ID
	                   ,(
	           SELECT TOP 1 dbo.PatientSex(lpi.Sex)
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = p.Patient_ID
	       ) AS Sex_Name
	           FROM   L_PATIENT_INFO P -- left JOIN tblYeucauXetnghiem_VNIO t2 ON t2.maBenhNhan=p.PID
	           WHERE  -- t2.maBenhNhan=P.PID
	              	--and   (t2.idCanLamSangThucHien = 47 )
			
	             EXISTS(
	                      SELECT *
	                      FROM   T_TEST_INFO
	                      WHERE  (@TestTypeID=-3 OR TESTTYPE_ID=@TestTypeID)
	                             AND Patient_ID = P.PAtient_ID
	                             AND CONVERT(NVARCHAR(10) ,TEST_DATE ,103) = @pTestDate
	                  )
	                  AND (
	                          (@HasTest=1)
	                          OR (
	                                 @HasTest=0
	                                 AND (
	                                         EXISTS(
	                                             SELECT 1
	                                             FROM   D_DATA_CONTROL
	                                             WHERE  DEvice_ID IN (SELECT 
	                                                                         device_ID
	                                                                  FROM   
	                                                                         D_Device_List 
	                                                                         D
	                                                                  WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID))
	                                                    AND UPPER(Alias_Name)NOT IN (SELECT 
	                                                                                        UPPER(Para_name)
	                                                                                 FROM   
	                                                                                        T_RESULT_DETAIL 
	                                                                                        T
	                                                                                 WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID)
	                                                                                        AND 
	                                                                                            Patient_ID = 
	                                                                                            P.PAtient_ID
	                                                                                        AND 
	                                                                                            CONVERT(NVARCHAR(10) , TEST_DATE , 103) = 
	                                                                                            @pTestDate
	                                                                                        AND 
	                                                                                            PARA_STATUS = 
	                                                                                            1)
	                                         )
	                                         AND EXISTS--kiem tra dangky XN 2 chieu
	                                             (
	                                                 SELECT *
	                                                 FROM   T_REG_LIST
	                                                 WHERE  TEST_ID IN (SELECT 
	                                                                           TEST_ID
	                                                                    FROM   
	                                                                           T_TEST_INFO
	                                                                    WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID)
	                                                                           AND 
	                                                                               Patient_ID = 
	                                                                               P.PAtient_ID
	                                                                           AND 
	                                                                               CONVERT(NVARCHAR(10) , TEST_DATE , 103) = 
	                                                                               @pTestDate)
	                                                        AND UPPER(Para_name) 
	                                                            NOT IN (SELECT 
	                                                                           UPPER(Para_name)
	                                                                    FROM   
	                                                                           T_RESULT_DETAIL 
	                                                                           T
	                                                                    WHERE  (@TestTypeID = - 3 OR TESTTYPE_ID = @TestTypeID)
	                                                                           AND 
	                                                                               Patient_ID = 
	                                                                               P.PAtient_ID
	                                                                           AND 
	                                                                               CONVERT(NVARCHAR(10) , TEST_DATE , 103) = 
	                                                                               @pTestDate
	                                                                           AND 
	                                                                               PARA_STATUS = 
	                                                                               1)
	                                             )--end Or
	                                     )--end exists
	                             )--end OR (@HasTest=0
	                      )
	                  AND (@PID='NOTHING' OR PID=@PID)
	                  AND (
	                          @Patient_Name='NOTHING'
	                          OR Patient_Name LIKE '%'+@Patient_Name+'%'
	                      )
	                  AND (@Age=0 OR Age=@Age)
	                  AND (@Sex=3 OR Sex=@Sex)
	       )P1
	       
	      
	ORDER BY
	       P1.Barcode ASC


