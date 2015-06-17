
ALTER PROC [dbo].[VIENMAT_BAOCAO_XN_TONGHOP]
  @pTestDateFrom  DATETIME
  ,@pTestDateTo    DATETIME
  ,@noitru         INT
  ,@dichvu         VARCHAR(500)
  ,@testTypeId      NVARCHAR(10),
  @ReportType_ID  NVARCHAR(10)
   AS
   
IF (@ReportType_ID = '0')
BEGIN
    SELECT U.*,
           (U.BH_NGOAITRU + U.BH_NOITRU + U.DV_NGOAITRU + U.DV_NOITRU) AS TONG
    FROM   (
               SELECT Y.TestType_Name,
                      Y.DataName,
                      0 AS Mien,
                      SUM(ISNULL(Y.BH_NGOAITRU, 0)) AS BH_NGOAITRU,
                      SUM(ISNULL(Y.BH_NOITRU, 0)) AS BH_NOITRU,
                      SUM(ISNULL(Y.DV_NGOAITRU, 0)) AS DV_NGOAITRU,
                      SUM(ISNULL(Y.DV_NOITRU, 0)) AS DV_NOITRU
               FROM   (
                          SELECT --COUNT(DISTINCT(T.Barcode))AS OK,
                                 
                                 T.Barcode,
                                 T.TestType_Name,
                                 T.DataName,
                                 T.ObjectType_ID,
                                 (
                                     CASE T.DichVu
                                          WHEN 0 THEN CASE T.NOITRU
                                                           WHEN 0 THEN ISNULL(T.UpdateNum, 1)
                                                      END
                                     END
                                 ) AS BH_NGOAITRU,
                                 (
                                     CASE T.DichVu
                                          WHEN 0 THEN CASE T.NOITRU
                                                           WHEN 1 THEN ISNULL(T.UpdateNum, 1)
                                                      END
                                     END
                                 ) AS BH_NOITRU,
                                 --(
                                 --    CASE T.DichVu
                                 --         WHEN 1 THEN CASE T.NOITRU
                                 --                          WHEN 0 THEN ISNULL(T.UpdateNum, 1)
                                 --                     END
                                 --    END
                                 --) AS DV_NGOAITRU,
                                 0 AS DV_NGOAITRU,
                                 
                                 --(
                                 --    CASE T.DichVu
                                 --         WHEN 1 THEN CASE T.NOITRU
                                 --                          WHEN 1 THEN ISNULL(T.UpdateNum, 1)
                                 --                     END
                                 --    END
                                 --) AS DV_NOITRU
                                 0 AS DV_NOITRU
                                 
                          FROM   (
                                     SELECT trd.TestType_ID,
                                            (
                                                SELECT TOP 1 T.TestType_Name
                                                FROM   T_TEST_TYPE_LIST T
                                                WHERE  T.TestType_ID = trd.TestType_ID
                                            )TestType_Name,
                                            (
                                                SELECT TOP 1 ddc.Data_Name
                                                FROM   D_DATA_CONTROL ddc
                                                WHERE  ddc.Data_Name = trd.Para_Name
                                            ) AS DataName,
                                            (
                                                SELECT TOP 1 lot.ID
                                                FROM   L_ObjectType lot
                                                WHERE  EXISTS(
                                                           SELECT 1
                                                           FROM   L_PATIENT_INFO 
                                                                  lpi
                                                           WHERE  lpi.ObjectType = 
                                                                  lot.ID
                                                                  AND lpi.Patient_ID = 
                                                                      trd.Patient_ID
                                                       )
                                            ) AS ObjectType_ID,
                                            (
                                                SELECT TOP 1 Y.dichVu
                                                FROM   
                                                       tblHisLis_PatientInfo_VNIO 
                                                       Y
                                                WHERE  EXISTS (
                                                           SELECT 1
                                                           FROM   L_PATIENT_INFO 
                                                                  lpi
                                                           WHERE  lpi.PID = Y.maBenhNhan
                                                                  AND lpi.Patient_ID = 
                                                                      trd.Patient_ID
                                                       )
                                            ) AS DichVu,
                                            (
                                                SELECT TOP 1 V.noiTru
                                                FROM   
                                                       tblHisLis_PatientInfo_VNIO 
                                                       V
                                                WHERE  EXISTS(
                                                           SELECT 1
                                                           FROM   L_PATIENT_INFO 
                                                                  lpi
                                                           WHERE  lpi.PID = V.maBenhNhan
                                                                  AND lpi.Patient_ID = 
                                                                      trd.Patient_ID
                                                       )
                                            ) AS NOITRU,
                                            trd.Barcode,
                                            ISNULL(trd.UpdateNum, 1) AS 
                                            UpdateNum
                                     FROM   T_RESULT_DETAIL trd
                                     WHERE  EXISTS(
                                                SELECT 1
                                                FROM   L_PATIENT_INFO lpi
                                                WHERE  lpi.Patient_ID = trd.Patient_ID
                                                       AND EXISTS(
                                                               SELECT 1
                                                               FROM   
                                                                      tblHisLis_PatientInfo_VNIO 
                                                                      V
                                                               WHERE  V.maBenhNhan = 
                                                                      lpi.PID
                                                                      AND (V.noiTru = @noitru OR @noitru = - 1)
                                                                          --AND (V.dichVu = @dichvu OR @dichvu = - 1)
                                                                      AND (
                                                                              (
                                                                                  V.idDoiTuongBenhNhan IN (SELECT 
                                                                                                                  *
                                                                                                           FROM   
                                                                                                                  ConvertStringtoIntTable(@dichvu))
                                                                              )                                                                             
                                                                          )
                                                                      AND EXISTS(
                                                                              SELECT 
                                                                                     1
                                                                              FROM   
                                                                                     tblYeucauXetnghiem_VNIO 
                                                                                     Y
                                                                              WHERE  
                                                                                     Y.maBenhNhan = 
                                                                                     lpi.PID
                                                                          )
                                                                      AND trd.Patient_ID 
                                                                          > 0
                                                                          --                                                                  AND (
                                                                          --                                                                          (
                                                                          --                                                                              @ReportType_ID=0 AND (trd.UpdateNum = 1)
                                                                          --                                                                              OR (@ReportType_ID = 1 AND 1 = 1)
                                                                          --                                                                          )
                                                                          --                                                                      )
                                                                      AND dbo.trunc(trd.Test_Date)
                                                                          BETWEEN 
                                                                          dbo.trunc(@pTestDateFrom) 
                                                                          AND 
                                                                          dbo.trunc(@pTestDateTo)
                                                           )
                                            )
                                            --  AND( trd.TestType_ID=@testTypeId OR @testTypeId=-1)
                                            AND (
                                                    @testTypeId = '-1'
                                                    OR trd.TestType_ID IN (SELECT 
                                                                                  *
                                                                           FROM   
                                                                                  dbo.ConvertStringtoIntTable(@testTypeId))
                                                )
                                 ) AS T
                      ) AS y
               GROUP BY
                      y.TestType_Name,
                      y.DataName
           ) AS U
    ORDER BY
           U.DataName DESC
END
ELSE
    SELECT U.*,
           (U.BH_NGOAITRU + U.BH_NOITRU + U.DV_NGOAITRU + U.DV_NOITRU) AS TONG
    FROM   (
               SELECT Y.TestType_Name,
                      Y.DataName,
                      0 AS Mien,
                      SUM(ISNULL(Y.BH_NGOAITRU, 0)) AS BH_NGOAITRU,
                      SUM(ISNULL(Y.BH_NOITRU, 0)) AS BH_NOITRU,
                      SUM(ISNULL(Y.DV_NGOAITRU, 0)) AS DV_NGOAITRU,
                      SUM(ISNULL(Y.DV_NOITRU, 0)) AS DV_NOITRU
               FROM   (
                          SELECT --COUNT(DISTINCT(T.Barcode))AS OK,
                                 
                                 T.Barcode,
                                 T.TestType_Name,
                                 T.DataName,
                                 T.ObjectType_ID,
                                 (
                                     CASE T.DichVu
                                          WHEN 0 THEN CASE T.NOITRU
                                                           WHEN 0 THEN (CASE WHEN ISNULL(T.UpdateNum, 1) >= 1 THEN 1 END)
                                                      END
                                     END
                                 ) AS BH_NGOAITRU,
                                 (
                                     CASE T.DichVu
                                          WHEN 0 THEN CASE T.NOITRU
                                                           WHEN 1 THEN (CASE WHEN ISNULL(T.UpdateNum, 1) >= 1 THEN 1 END)
                                                      END
                                     END
                                 ) AS BH_NOITRU,
                                 --(
                                 --    CASE T.DichVu
                                 --         WHEN 1 THEN CASE T.NOITRU
                                 --                          WHEN 0 THEN (CASE WHEN ISNULL(T.UpdateNum, 1) >= 1 THEN 1 END)
                                 --                     END
                                 --    END
                                 --) AS DV_NGOAITRU,
                                 0 AS DV_NGOAITRU,
                                 --(
                                 --    CASE T.DichVu
                                 --         WHEN 1 THEN CASE T.NOITRU
                                 --                          WHEN 1 THEN (CASE WHEN ISNULL(T.UpdateNum, 1) >= 1 THEN 1 END)
                                 --                     END
                                 --    END
                                 --) AS DV_NOITRU
                                 0 AS DV_NOITRU
                          FROM   (
                                     SELECT trd.TestType_ID,
                                            (
                                                SELECT TOP 1 T.TestType_Name
                                                FROM   T_TEST_TYPE_LIST T
                                                WHERE  T.TestType_ID = trd.TestType_ID
                                            )TestType_Name,
                                            (
                                                SELECT TOP 1 ddc.Data_Name
                                                FROM   D_DATA_CONTROL ddc
                                                WHERE  ddc.Data_Name = trd.Para_Name
                                            ) AS DataName,
                                            (
                                                SELECT TOP 1 lot.ID
                                                FROM   L_ObjectType lot
                                                WHERE  EXISTS(
                                                           SELECT 1
                                                           FROM   L_PATIENT_INFO 
                                                                  lpi
                                                           WHERE  lpi.ObjectType = 
                                                                  lot.ID
                                                                  AND lpi.Patient_ID = 
                                                                      trd.Patient_ID
                                                       )
                                            ) AS ObjectType_ID,
                                            (
                                                SELECT TOP 1 Y.dichVu
                                                FROM   
                                                       tblHisLis_PatientInfo_VNIO 
                                                       Y
                                                WHERE  EXISTS (
                                                           SELECT 1
                                                           FROM   L_PATIENT_INFO 
                                                                  lpi
                                                           WHERE  lpi.PID = Y.maBenhNhan
                                                                  AND lpi.Patient_ID = 
                                                                      trd.Patient_ID
                                                       )
                                            ) AS DichVu,
                                            (
                                                SELECT TOP 1 V.noiTru
                                                FROM   
                                                       tblHisLis_PatientInfo_VNIO 
                                                       V
                                                WHERE  EXISTS(
                                                           SELECT 1
                                                           FROM   L_PATIENT_INFO 
                                                                  lpi
                                                           WHERE  lpi.PID = V.maBenhNhan
                                                                  AND lpi.Patient_ID = 
                                                                      trd.Patient_ID
                                                       )
                                            ) AS NOITRU,
                                            trd.Barcode,
                                            ISNULL(trd.UpdateNum, 1) AS 
                                            UpdateNum
                                     FROM   T_RESULT_DETAIL trd
                                     WHERE  EXISTS(
                                                SELECT 1
                                                FROM   L_PATIENT_INFO lpi
                                                WHERE  lpi.Patient_ID = trd.Patient_ID
                                                       AND EXISTS(
                                                               SELECT 1
                                                               FROM   
                                                                      tblHisLis_PatientInfo_VNIO 
                                                                      V
                                                               WHERE  V.maBenhNhan = 
                                                                      lpi.PID
                                                                      AND (V.noiTru = @noitru OR @noitru = - 1)
                                                                          --AND (V.dichVu = @dichvu OR @dichvu = - 1)
                                                                     AND (
                                                                              (
                                                                                  V.idDoiTuongBenhNhan IN (SELECT 
                                                                                                                  *
                                                                                                           FROM   
                                                                                                                  ConvertStringtoIntTable(@dichvu))
                                                                              )
                                                                          )
                                                                      AND EXISTS(
                                                                              SELECT 
                                                                                     1
                                                                              FROM   
                                                                                     tblYeucauXetnghiem_VNIO 
                                                                                     Y
                                                                              WHERE  
                                                                                     Y.maBenhNhan = 
                                                                                     lpi.PID
                                                                          )
                                                                      AND trd.Patient_ID 
                                                                          > 0
                                                                          --                                                                  AND (
                                                                          --                                                                          (
                                                                          --                                                                              @ReportType_ID=0 AND (trd.UpdateNum = 1)
                                                                          --                                                                              OR (@ReportType_ID = 1 AND 1 = 1)
                                                                          --                                                                          )
                                                                          --                                                                      )
                                                                      AND dbo.trunc(trd.Test_Date)
                                                                          BETWEEN 
                                                                          dbo.trunc(@pTestDateFrom) 
                                                                          AND 
                                                                          dbo.trunc(@pTestDateTo)
                                                           )
                                            )
                                            --  AND( trd.TestType_ID=@testTypeId OR @testTypeId=-1)
                                            AND (
                                                    @testTypeId = '-1'
                                                    OR trd.TestType_ID IN (SELECT 
                                                                                  *
                                                                           FROM   
                                                                                  dbo.ConvertStringtoIntTable(@testTypeId))
                                                )
                                 ) AS T
                      ) AS y
               GROUP BY
                      y.TestType_Name,
                      y.DataName
           ) AS U
    ORDER BY
           U.DataName DESC