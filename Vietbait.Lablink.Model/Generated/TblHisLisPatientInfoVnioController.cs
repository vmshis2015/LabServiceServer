using System;
using System.ComponentModel;
using System.Threading;
using System.Web;
using SubSonic;

// <auto-generated />

namespace Vietbait.Lablink.Model
{
    /// <summary>
    ///     Controller class for tblHisLis_PatientInfo_VNIO
    /// </summary>
    [DataObject]
    public class TblHisLisPatientInfoVnioController
    {
        // Preload our schema..
        private TblHisLisPatientInfoVnio thisSchemaLoad = new TblHisLisPatientInfoVnio();
        private string userName = String.Empty;

        protected string UserName
        {
            get
            {
                if (userName.Length == 0)
                {
                    if (HttpContext.Current != null)
                    {
                        userName = HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userName = Thread.CurrentPrincipal.Identity.Name;
                    }
                }
                return userName;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TblHisLisPatientInfoVnioCollection FetchAll()
        {
            var coll = new TblHisLisPatientInfoVnioCollection();
            var qry = new Query(TblHisLisPatientInfoVnio.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblHisLisPatientInfoVnioCollection FetchByID(object Id)
        {
            TblHisLisPatientInfoVnioCollection coll = new TblHisLisPatientInfoVnioCollection().Where("Id", Id).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TblHisLisPatientInfoVnioCollection FetchByQuery(Query qry)
        {
            var coll = new TblHisLisPatientInfoVnioCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (TblHisLisPatientInfoVnio.Delete(Id) == 1);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (TblHisLisPatientInfoVnio.Destroy(Id) == 1);
        }


        /// <summary>
        ///     Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(string BacSyDieuTri, string Buong, string DiaChi, bool? GioiTinh, string Giuong,
            string IdBacSyDieuTri, short? IdKhoa, string Khoa, string MaBenhNhan, bool? NoiTru, string Phong,
            string TenBenhNhan, short? Tuoi, string Sophieu, DateTime? TestDate, string Datra, bool? DichVu,
            int? IdDoiTuongBenhNhan, string MaLanKham)
        {
            var item = new TblHisLisPatientInfoVnio();

            item.BacSyDieuTri = BacSyDieuTri;

            item.Buong = Buong;

            item.DiaChi = DiaChi;

            item.GioiTinh = GioiTinh;

            item.Giuong = Giuong;

            item.IdBacSyDieuTri = IdBacSyDieuTri;

            item.IdKhoa = IdKhoa;

            item.Khoa = Khoa;

            item.MaBenhNhan = MaBenhNhan;

            item.NoiTru = NoiTru;

            item.Phong = Phong;

            item.TenBenhNhan = TenBenhNhan;

            item.Tuoi = Tuoi;

            item.Sophieu = Sophieu;

            item.TestDate = TestDate;

            item.Datra = Datra;

            item.DichVu = DichVu;

            item.IdDoiTuongBenhNhan = IdDoiTuongBenhNhan;

            item.MaLanKham = MaLanKham;


            item.Save(UserName);
        }

        /// <summary>
        ///     Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(long Id, string BacSyDieuTri, string Buong, string DiaChi, bool? GioiTinh, string Giuong,
            string IdBacSyDieuTri, short? IdKhoa, string Khoa, string MaBenhNhan, bool? NoiTru, string Phong,
            string TenBenhNhan, short? Tuoi, string Sophieu, DateTime? TestDate, string Datra, bool? DichVu,
            int? IdDoiTuongBenhNhan, string MaLanKham)
        {
            var item = new TblHisLisPatientInfoVnio();
            item.MarkOld();
            item.IsLoaded = true;

            item.Id = Id;

            item.BacSyDieuTri = BacSyDieuTri;

            item.Buong = Buong;

            item.DiaChi = DiaChi;

            item.GioiTinh = GioiTinh;

            item.Giuong = Giuong;

            item.IdBacSyDieuTri = IdBacSyDieuTri;

            item.IdKhoa = IdKhoa;

            item.Khoa = Khoa;

            item.MaBenhNhan = MaBenhNhan;

            item.NoiTru = NoiTru;

            item.Phong = Phong;

            item.TenBenhNhan = TenBenhNhan;

            item.Tuoi = Tuoi;

            item.Sophieu = Sophieu;

            item.TestDate = TestDate;

            item.Datra = Datra;

            item.DichVu = DichVu;

            item.IdDoiTuongBenhNhan = IdDoiTuongBenhNhan;

            item.MaLanKham = MaLanKham;

            item.Save(UserName);
        }
    }
}