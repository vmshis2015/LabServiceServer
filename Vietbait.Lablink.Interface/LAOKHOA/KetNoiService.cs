using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using Vietbait.Lablink.Model;
using VietBaIT.CommonLibrary;
using Vietbait.Lablink.Utilities;
using YkhoaNet.HisLink.DataAccessLayer;
using System.Data;
using System.Transactions;
using SPs = YkhoaNet.HisLink.DataAccessLayer.SPs;

namespace Vietbait.Lablink.Interface.LAOKHOA
{
    public class KetNoiService : CommonBusiness
   {

       //public string VienPhi = "";

       private string NghiepVuY =
           "workstation id=192.168.1.254;packet size=4096;data source=192.168.1.254;persist security info=False;initial catalog=LK_NghiepVuY;uid=hislink;pwd=hislink";
       /// <summary>
       /// hàm thực hiện việc lấy thông tin của bệnh nhân từ bên db của hislink
       /// </summary>
       /// <param name="MaPhieu">mã phiếu chỉ định truyền vào</param>
       /// <returns>dữ liệu danh sách bệnh nhân</returns>
       public DataSet LayThongTinBenhNhan(string MaPhieu)
       {
           DataSet dataTable = new DataSet();
           using(var sh=new SharedDbConnectionScope(NghiepVuY))
           {

               dataTable = SPs.SpGetPatientTestYkhoaNet(MaPhieu).GetDataSet();

           }
           return dataTable;
       }
        /// <summary>
        /// hàm thực hiện việc update thông tin của kiểu dịch vụ
        /// </summary>
        /// <param name="dataTable"></param>
       public void UpdateMapTestTypeList(ref  DataTable dataTable)
       {
           if(!dataTable.Columns.Contains("TestType_Name")) dataTable.Columns.Add("TestType_Name", typeof (string));
           foreach (DataRow drv in dataTable.Rows)
           {
               drv["MaKhoa"] = drv["MaKhoa"].ToString().Trim();
               drv["TestType_Name"] = GetTestTypeList(drv["MaKhoa"].ToString().Trim());

           }
           dataTable.AcceptChanges();
       }
        /// <summary>
        /// hàm thực hiện việc lấy kiểu dịch vụ
        /// </summary>
        /// <param name="MaDV"></param>
        /// <returns></returns>
       private string GetTestTypeList(string MaDV)
       {
           string TestType_Name = "";
           SqlQuery sqlQuery = new Select().From(TTestTypeList.Schema)
               .Where(TTestTypeList.Columns.Abbreviation).IsEqualTo(MaDV.Trim());
           TTestTypeList objTestTypeList = sqlQuery.ExecuteSingle<TTestTypeList>();
           if (objTestTypeList != null) TestType_Name = objTestTypeList.TestTypeName;

           return TestType_Name;
       }
        /// <summary>
        /// hàm thực hiện việc load thông tin của 2 hàm chi tiết
        /// </summary>
        /// <param name="Alias_Name"></param>
        /// <param name="Testtype_Id"></param>
        /// <returns></returns>
       public DataTable SearchDataParaInDatacontrol(string Alias_Name,int Testtype_Id)
       {
           return Model.SPs.LaokhoaGetAllDetailTypeList(Testtype_Id, Alias_Name).GetDataSet().Tables[0];
       }
        public DataTable GetThietBi()
        {
            return new Select().From(DDeviceList.Schema)
                .OrderAsc(DDeviceList.Columns.DeviceName).ExecuteDataSet().Tables[0];
        }
        /// <summary>
        /// hàm thực hiện việc tìm kiếm thông tin của dịch vụ
        /// </summary>
        /// <param name="MaDV"></param>
        /// <param name="TenDV"></param>
        /// <returns></returns>
        public DataTable SearchDataInDV(string MaDV,string TenDV)
        {
            DataTable dataTable=new DataTable();
            using (var sh = new SharedDbConnectionScope(NghiepVuY))
            {
                dataTable = SPs.KetnoiSearchThongDv(MaDV, TenDV).GetDataSet().Tables[0];
            }
            return dataTable;
        }
       
   }
}
