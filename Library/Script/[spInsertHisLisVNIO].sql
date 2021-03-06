/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.0.70
 * Time: 23/04/2012 17:19:29
 ************************************************************/

ALTER PROCEDURE [dbo].[spInsertHisLisVNIO](
    @MaBN             NVARCHAR(50),
    @pTenBenhNhan     NVARCHAR(100),
    @pTuoi            INT,
    @pGioitinh        BIT,
    @pDiachi          NVARCHAR(150),
    @pGiuong          NVARCHAR(50),
    @pKhoa            NVARCHAR(50),
    @pIdKhoa          INT,
    @pBuong           NVARCHAR(100),
    @pIdbacSydieuTri  INT,
    @pBacSyDieuTri    NVARCHAR(100),
    @pNoitru          BIT,
    @pPhong           NVARCHAR(20),
    @pTestdate        DATETIME,
    @psophieu         NVARCHAR(50),
    @pdatra           NVARCHAR(20),
    @pdichVu          BIT,
    @idDoiTuongBenhNhan INT
)
AS
	IF NOT EXISTS (
	       SELECT *
	       FROM   tblHisLis_PatientInfo_VNIO
	       WHERE  maBenhNhan = @MaBN
	   )
	    INSERT INTO tblHisLis_PatientInfo_VNIO
	      (
	        maBenhNhan,
	        tenBenhNhan,
	        tuoi,
	        gioiTinh,
	        diaChi,
	        giuong,
	        khoa,
	        idKhoa,
	        buong,
	        idBacSyDieuTri,
	        bacSyDieuTri,
	        noitru,
	        phong,
	        test_date,
	        sophieu,
	        datra,
	        dichVu,
	        idDoiTuongBenhNhan
	      )
	    VALUES
	      (
	        @MaBN,
	        @pTenBenhNhan,
	        @pTuoi,
	        @pGioitinh,
	        @pDiachi,
	        @pGiuong,
	        @pKhoa,
	        @pIdKhoa,
	        @pBuong,
	        @pIdbacSydieuTri,
	        @pBacSyDieuTri,
	        @pNoitru,
	        @pPhong,
	        dbo.trunc(@pTestdate),
	        @psophieu,
	        @pdatra,
	        @pdichVu,
	        @idDoiTuongBenhNhan
	      )
	ELSE
	    UPDATE tblHisLis_PatientInfo_VNIO
	    SET    tenBenhNhan     = @pTenBenhNhan,
	           tuoi            = @pTuoi,
	           gioiTinh        = @pGioitinh,
	           diaChi          = @pDiachi,
	           khoa            = @pKhoa,
	           idKhoa          = @pIdKhoa,
	           buong           = @pBuong,
	           idBacSyDieuTri  = @pIdbacSydieuTri,
	           bacSyDieuTri    = @pBacSyDieuTri,
	           noiTru          = @pNoitru,
	           phong           = @pPhong,
	           test_date       = dbo.trunc(@pTestdate),
	           sophieu         = @psophieu,
	           datra           = @pdatra,
	           dichVu          = @pdichVu,
	           idDoiTuongBenhNhan = @idDoiTuongBenhNhan
	    WHERE  maBenhNhan      = @MaBN
