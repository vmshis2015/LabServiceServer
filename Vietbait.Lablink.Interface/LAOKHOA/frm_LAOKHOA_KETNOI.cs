using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VietBaIT.CommonLibrary;
namespace Vietbait.Lablink.Interface.LAOKHOA
{
    public partial class frm_LAOKHOA_KETNOI : Form
    {
        #region "khai báo biến private"
        private DataTable m_dtPatientInfo=new DataTable();
        private DataTable m_dtTestInfo=new DataTable();
        private DataSet ds=new DataSet();
        #endregion
        public frm_LAOKHOA_KETNOI()
        {
            InitializeComponent();
        }
        /// <summary>
        /// hàm thực hiện dùng phím tắt của form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_LAOKHOA_KETNOI_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)cmdExit.PerformClick();
            if(e.KeyCode==Keys.S&&e.Control)cmdSave.PerformClick();
            if(e.KeyCode==Keys.F3)cmdGetData.PerformClick();
        }
        /// <summary>
        /// hàm thực hiện thoát khỏi form hiện tại
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// hàm thực hiện việc lấy thông tin của dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdGetData_Click(object sender, EventArgs e)
        {
            if (!InValid())return;
            GetData();
        }

        #region "khai báo các hàm dùng chung"
        /// <summary>
        /// hàm thực hiện việc kiểm tra thông tin của mã phiếu
        /// </summary>
        /// <returns></returns>
        private bool InValid()
        {
            if(string.IsNullOrEmpty(txtMaChiDinh.Text))
            {
                Utility.ShowMsg("Bạn phải nhập mã chỉ định","Thông báo",MessageBoxIcon.Warning);
                txtMaChiDinh.Focus();
                return false;
            }
            return true;
        }
        /// <summary>
        /// hàm thực hiện lấy thông tin khi nhấn nút tìm kiếm hoặc dùng phím tắt F3
        /// </summary>
        void GetData()
        {
            ds = new KetNoiService().LayThongTinBenhNhan(Utility.sDbnull(txtMaChiDinh.Text, ""));
            m_dtPatientInfo = ds.Tables[0];
            m_dtTestInfo = ds.Tables[1];
            new KetNoiService().UpdateMapTestTypeList(ref  m_dtTestInfo);
            grdPatientInfo.DataSource = m_dtPatientInfo;
            grdTestInfo.DataSource = m_dtTestInfo;
        }
        #endregion

        private void frm_LAOKHOA_KETNOI_Load(object sender, EventArgs e)
        {
            txtMaChiDinh.Text = "CD090701001";
        }
    }
}
