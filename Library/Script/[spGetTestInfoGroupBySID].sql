set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go






/************************************************************
 * Code formatted by SoftTree SQL Assistant © v5.1.7
 * Time: 01/04/2011 14:47:58
 ************************************************************/

-- Procedure
ALTER PROCEDURE [dbo].[spGetTestInfoGroupBySID] 
(
    @pTestDateFrom   NVARCHAR(10),
    @pTestDateTo     NVARCHAR(10),
    @strTestType_ID  NVARCHAR(50),
    @Barcode         NVARCHAR(50),
    @PID             NVARCHAR(50),
    @Patient_Name    NVARCHAR(50),
    @Age             INT,
    @Sex             INT,
    @strTestStatus   NVARCHAR(50)
)
AS
	SELECT  --DISTINCT 1 AS CHON,
	        --tti.Barcode,
	        
	         DISTINCT tti.Patient_ID,	 
       (
	           SELECT TOP 1 tti2.Barcode
	           FROM  T_TEST_INFO tti2
	           WHERE tti.Patient_ID = tti2.Patient_ID
	           ORDER BY tti.Barcode DESC
	       ) AS Barcode,
	       (
	           SELECT TOP 1 lpi.Patient_Name
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID
	       ) AS Patient_Name,
	       (
	           SELECT TOP 1 lpi.Age
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID
	       ) AS Age,
	       (
	           SELECT TOP 1 lpi.PID
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID
	       ) AS PID,
	       (
	           SELECT TOP 1 lpi.DATEUPDATE
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID 
	       ) AS DATEUPDATE,
	       (
	           SELECT TOP 1 dbo.PatientSex(lpi.Sex)
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID
	       ) AS Sex_Name,
	       
	       (
	           SELECT TOP 1 lpi.Diagnostic
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID
	       ) AS Diagnostic,
	        (
	           SELECT TOP 1 lpi.bed
	           FROM   L_PATIENT_INFO lpi
	           WHERE  lpi.Patient_ID = tti.Patient_ID
	       ) AS bed
	       ,dbo.GetPrintStatus(tti.Patient_ID,@pTestDateFrom,@pTestDateTo) as Printstatus
,dbo.GetNewPrintStatus(tti.Patient_ID,@pTestDateFrom,@pTestDateTo) as NewPrintstatus
--	      , (
--	           SELECT TOP 1  dbo.ChangeColor(tti.TestType_ID)
--	           FROM   L_PATIENT_INFO lpi
--	           WHERE  lpi.Patient_ID = tti.Patient_ID
--	       )  AS Mau
	       
	FROM   T_TEST_INFO tti 
	WHERE  (
	           CONVERT(DATETIME, CONVERT(NVARCHAR(10), tti.TEST_DATE, 103), 103) 
	           BETWEEN CONVERT(DATETIME, @pTestDateFrom, 103)
	           
	           AND
	           CONVERT(DATETIME, @pTestDateTo, 103)
	       )
	       AND (
	               @strTestType_ID = '-3'
	               OR tti.TestType_ID IN (SELECT *
	                                      FROM   dbo.ConvertStringtoIntTable(@strTestType_ID))
	           )
	       AND (
	               @strTestStatus = '-3'
	               OR tti.Test_Status IN (SELECT *
	                                      FROM   dbo.ConvertStringtoIntTable(@strTestStatus))
	           )
	       AND (
	               tti.Barcode LIKE '%' + @Barcode + '%'
	               OR @Barcode = 'NOTHING'
	           )
	       AND (EXISTS(
	               SELECT 1
	               FROM   L_PATIENT_INFO lpi
	               WHERE  (lpi.Patient_ID = tti.Patient_ID)
	                      AND (
	                              lpi.Patient_Name LIKE '%' + @Patient_Name +
	                              '%'
	                              OR @Patient_Name = 'NOTHING'
	                          )
	                      AND (lpi.Age = @Age OR @Age = 0)
	                      AND (lpi.Sex = @Sex OR @Sex = -1)
	                      AND (lpi.PID LIKE '%' + @PID + '%' OR @PID = 'NOTHING')
 AND CONVERT(DATETIME, CONVERT(NVARCHAR(10), tti.TEST_DATE, 103), 103) 
	           BETWEEN CONVERT(DATETIME, @pTestDateFrom, 103)
	           
	           AND
	           CONVERT(DATETIME, @pTestDateTo, 103)

	       )
	       or tti.Patient_ID > 0
	       )
	--ORDER BY tti.Barcode desc






