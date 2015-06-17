alter PROCEDURE sp_ReportDailyBNVNIO
(
	@pTestDateFrom nvarchar(10),
@pTestDateTo  nvarchar(10),
--@pObject NVARCHAR(10),
--@pTestType_ID NVARCHAR(10),
--@pDoctor NVARCHAR(20),
@pDepartMent NVARCHAR(20)
	)as
SELECT distinct L.Patient_ID,L.PID,L.Patient_Name,ObjectType,L.Age,L.Sex,L.DATEUPDATE,t.idKhoa,t.khoa,t.bacSyDieuTri,t.idBacSyDieuTri--,tti.Test_Status,tti.TestType_ID
--(SELECT TOP 1 tti2.TestType_Name
--  FROM T_TEST_TYPE_LIST  tti2 WHERE tti.TestType_ID=tti2.TestType_ID
--	
--	)AS TestTypeName
FROM   L_PATIENT_INFO L
       JOIN tblHisLis_PatientInfo_VNIO t
            ON  L.PID = t.maBenhNhan
--       JOIN T_TEST_INFO tti
--            ON  tti.Patient_ID = L.Patient_ID
WHERE  L.PID = t.maBenhNhan
      -- AND tti.TestType_ID =@pTestType_ID
       AND    dbo.trunc(DATEUPDATE)BETWEEN dbo.trunc(@pTestDateFrom) 
	                  AND dbo.trunc (@pTestDateTo) 
	                    --   AND (@pTestType_ID='-1' OR TESTTYPE_ID IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pTestType_ID) ))
 --AND (@pDoctor='-1' OR idBacSyDieuTri IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pDoctor) ))
 AND (@pDepartMent='-1' OR idKhoa IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pDepartMent) ))
 --AND (@pObject='-1' OR ObjectType IN (SELECT * FROM dbo.ConvertStringtoIntTable(@pObject) ))

      -- AND dbo.trunc(tti.Test_Date) = '2012-02-14 00:00:00:000' --AND  t.idbacsydieutri=213--t.idkhoa=2      --AND lpi.DATEUPDATE = '2012-02-14 00:00:00.000'
ORDER BY t.bacSyDieuTri
      -- AND lpi.Patient_ID>0
      SELECT DISTINCT  idkhoa, khoa
        FROM tblHisLis_PatientInfo_VNIO WHERE test_date='2012-02-14'     
      SELECT DISTINCT 1 chon ,bacSyDieuTri, idBacSyDieuTri
        FROM tblHisLis_PatientInfo_VNIO WHERE idBacSyDieuTri<>0 ---WHERE test_date='2012-02-14'
EXEC sp_ReportDailyBNVNIO @pTestDateFrom='2012-02-14',@pTestDateTo='2012-02-14',@pTestType_ID='2,3',@pDoctor='336,397,331,298',@pDepartMent='2,3,5,7',@pObject=-1

    EXEC sp_ReportDailyBNVNIO @pTestDateFrom='2012-02-14',@pTestDateTo='2012-02-14',@pDepartMent='7'