set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go






/************************************************************
 * Code formatted by SoftTree SQL Assistant © v4.0.34
 * Time: 02/02/2012 9:03:43 PM
 ************************************************************/

--


ALTER PROCEDURE [dbo].[sp_ReportAllVNIOV2](
    @pTestDateFrom  NVARCHAR(10)
   ,@pTestDateTo    NVARCHAR(10)
     ,@pTestType_ID   NVARCHAR(10)
   ,@ObjectType     NVARCHAR(10)
   --,@noingoaitru      INT
)

AS
	SELECT PARA_NAME
	      ,TESTTYPE_ID
	      ,TESTTYPE_NAME
	       ,UpdateNum AS Amount
	      ,PrintDetail
	      ,Tongbarcode,SumTest
	      ,(
	           SELECT COUNT(DISTINCT(patient_ID))
	           FROM   T_TEST_INFO T2
	           WHERE 
	                     dbo.fnFormatDate(Require_Date)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	            dbo.trunc(TEST_DATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo) 
--	                  	           CONVERT(DATETIME ,TEST_DATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
	                 -- AND TESTTYPE_ID = T1.TESTTYPE_ID
	                     AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))

	                  AND EXISTS (
	                          SELECT 1
	                          FROM   L_PATIENT_INFO
	                          WHERE  PATIENT_ID = T2.PATIENT_ID
	                              AND (@ObjectType='-1' OR ObjectType IN (SELECT * FROM dbo.ConvertStringtoIntTable(@ObjectType) ))

	                               
	                      )
	       ) AS NumOfTest
	       

	      ,(
	           SELECT COUNT(DISTINCT(patient_ID))
	           FROM   T_TEST_INFO T2
	           WHERE  
	                     dbo.fnFormatDate(Require_Date)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	           dbo.trunc(TEST_DATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo) 
--	                  	           CONVERT(DATETIME ,TEST_DATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
	                --  AND TESTTYPE_ID = T1.TESTTYPE_ID
	                AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))

	                  AND EXISTS (
	                          SELECT 1
	                          FROM   L_PATIENT_INFO
	                          WHERE  PATIENT_ID = T2.PATIENT_ID
--	                                 AND (@ObjectType=-1 OR ObjectType=@ObjectType)
   AND (@ObjectType='-1' OR ObjectType IN (SELECT * FROM dbo.ConvertStringtoIntTable(@ObjectType) ))

	                  )
	                  	                   AND EXISTS(SELECT 1 FROM L_PATIENT_INFO lpi WHERE T2.Patient_ID=lpi.Patient_ID
	                   AND EXISTS(SELECT 1 FROM tblHisLis_PatientInfo_VNIO V WHERE V.maBenhNhan=lpi.PID AND (V.noiTru=1 ) ))

	       ) AS Noitru
	       
	       ,(
	           SELECT COUNT(DISTINCT(patient_ID))
	           FROM   L_PATIENT_INFO T2
	           WHERE 
	                     dbo.fnFormatDate(T2.DATEUPDATE)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	            dbo.trunc(DATEUPDATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo) 
--	                  	           CONVERT(DATETIME ,DATEUPDATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
	                 -- AND TESTTYPE_ID = T1.TESTTYPE_ID
	                      AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))

	                  AND EXISTS (
	                          SELECT 1
	                          FROM   L_PATIENT_INFO
	                          WHERE  PATIENT_ID = T2.PATIENT_ID
--	                                 AND (@ObjectType=-1 OR ObjectType=@ObjectType)
   AND (@ObjectType='-1' OR ObjectType IN (SELECT * FROM dbo.ConvertStringtoIntTable(@ObjectType) ))

	                  )
	                  	--AND EXISTS(SELECT 1 FROM L_PATIENT_INFO lpi WHERE T2.Patient_ID=lpi.Patient_ID
	        	                   AND EXISTS(SELECT 1 FROM L_PATIENT_INFO lpi WHERE T2.Patient_ID=lpi.Patient_ID
	                   AND EXISTS(SELECT 1 FROM tblHisLis_PatientInfo_VNIO V WHERE V.maBenhNhan=lpi.PID AND (V.noiTru=0 ) ))

	       ) AS Ngoaitru    
	
	        ,(
	           SELECT COUNT(DISTINCT(patient_ID))
	           FROM   L_PATIENT_INFO T2
	           WHERE 
	                     dbo.fnFormatDate(T2.DATEUPDATE)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	            dbo.trunc(DATEUPDATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo) 
--	                  	           CONVERT(DATETIME ,DATEUPDATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
--	                 -- AND TESTTYPE_ID = T1.TESTTYPE_ID
	                  AND EXISTS (
	                          SELECT 1
	                          FROM   L_PATIENT_INFO
	                          WHERE  PATIENT_ID = T2.PATIENT_ID
	                               AND(@ObjectType='-1' OR  ObjectType IN (SELECT * FROM dbo.ConvertStringtoIntTable(@ObjectType)))
	                  )
	                  	                --   AND EXISTS(SELECT 1 FROM L_PATIENT_INFO lpi WHERE T2.Patient_ID=lpi.Patient_ID)
	                  -- AND EXISTS(SELECT 1 FROM tblHisLis_PatientInfo_VNIO V WHERE V.maBenhNhan=lpi.PID AND (V.noiTru=0 or V.noiTru=1 OR @noingoaitru=-1) ))
 AND EXISTS(SELECT 1 FROM tblHisLis_PatientInfo_VNIO V WHERE (v.dichVu =-1  ) )

	       ) AS Tongnoingoaitru
	       , (SELECT COUNT(DISTINCT(patient_ID))AS barcode 
	                       FROM T_RESULT_DETAIL trd  
	          WHERE
	                    dbo.fnFormatDate(TEST_DATE)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	           dbo.trunc(TEST_DATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo) 
--	                        CONVERT(DATETIME ,TEST_DATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
--	                  	
	                                       AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))

	                       		AND EXISTS (
	                          SELECT 1
	                          FROM   T_TEST_INFO tti
	                          WHERE  PATIENT_ID = trd.PATIENT_ID
	                                -- AND (@ObjectType=-1 OR ObjectType=@ObjectType)
	                      )
	                    ) AS Tong
	      
	      
	FROM   (
	           SELECT PARA_NAME
	                 ,TESTTYPE_ID
	                 ,(
	                      SELECT TESTTYPE_NAME
	                      FROM   T_TEST_TYPE_LIST
	                      WHERE  TESTTYPE_ID = T.TESTTYPE_ID
	                  ) AS TESTTYPE_NAME
	                  ,Sum(Case When t.UpdateNum =1 Then UpdateNum End) SumTest
--	                  ,        						SUM(UpdateNum) AS YENetSales

	                  ,SUM(ISNULL(UpdateNum ,1)) AS UpdateNum	--COUNT(PARA_NAME) AS Amount,	--COUNT(DISTINCT(PATIENT_ID)) as NumOfTest,
	                 ,(
	                      SELECT COUNT(*)
	                      FROM   D_DATA_CONTROL
	                      WHERE  DEVICE_ID IN (SELECT DEVICE_ID
	                                           FROM   D_DEVICE_LIST
	                                           WHERE  TESTTYPE_ID = T.TESTTYPE_ID)
	                  ) AS CountParam
	                 ,(
	                      SELECT PrintDetail
	                      FROM   T_TEST_TYPE_LIST
	                      WHERE  TESTTYPE_ID = T.TESTTYPE_ID
	                  ) AS PrintDetail
	                    , (SELECT COUNT(DISTINCT(patient_ID))AS barcode 
	                       FROM T_RESULT_DETAIL trd  
	                       WHERE 
	                         dbo.fnFormatDate(TEST_DATE)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	                        dbo.trunc(TEST_DATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo)
--	                        CONVERT(DATETIME ,TEST_DATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
	                  	
	                                       AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))

	                       		AND EXISTS (
	                          SELECT 1
	                          FROM   T_TEST_INFO tti
	                          WHERE  PATIENT_ID = trd.PATIENT_ID
	                                -- AND (@ObjectType=-1 OR ObjectType=@ObjectType)
	                      )
	                    ) AS Tongbarcode
	                    
	                   FROM   T_RESULT_DETAIL T
	           WHERE  
	             dbo.fnFormatDate(TEST_DATE)BETWEEN dbo.fnFormatDate(@pTestDateFrom) 
	                  AND dbo.fnFormatDate (@pTestDateTo) 
--	           dbo.trunc(TEST_DATE)BETWEEN dbo.trunc(@pTestDateFrom) 
--	                  AND dbo.trunc (@pTestDateTo)
--	                  	           CONVERT(DATETIME ,TEST_DATE ,103) BETWEEN CONVERT(DATETIME ,@pTestDateFrom ,103)
--	                  	                  AND CONVERT(DATETIME ,@pTestDateTo ,103)
	                     AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))

	                 -- AND (@pTestType_ID=-1 OR TESTTYPE_ID=@pTestType_ID)
	                  AND EXISTS (
	                          SELECT 1
	                          FROM   L_PATIENT_INFO
	                          WHERE  PATIENT_ID = T.PATIENT_ID
--	                                 AND (@ObjectType=-1 OR ObjectType=@ObjectType)

 AND(@ObjectType='-1' OR  ObjectType IN (SELECT * FROM dbo.ConvertStringtoIntTable(@ObjectType)))
	                  )
	                   AND EXISTS(SELECT 1 FROM L_PATIENT_INFO lpi WHERE T.Patient_ID=lpi.Patient_ID
	                   AND EXISTS(SELECT 1 FROM tblHisLis_PatientInfo_VNIO V WHERE V.maBenhNhan=lpi.PID AND (V.noiTru=1 OR V.noiTru=0 ) ))
	and  T.Patient_ID>0
	           GROUP BY
	                  PARA_NAME
	                 ,TESTTYPE_ID--,UpdateNum
	       )T1
	ORDER BY
	       TESTTYPE_ID
	      ,PARA_NAME DESC





