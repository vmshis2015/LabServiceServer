using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using VietBaIT.CommonLibrary;
using Vietbait.Lablink.Model;
namespace Vietbait.Lablink.Interface.LAOKHOA
{
    public partial class frm_MapingPara : Form
    {
        private  DataTable m_dtDataControl=new DataTable();
        private DataTable m_dtDataDichVu=new DataTable();
        public frm_MapingPara()
        {
            InitializeComponent();
        }
        /// <summary>
        /// hàm thực hiện dùng phím tắt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_MapingPara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) cmdExit.PerformClick();
            if (e.KeyCode == Keys.S && e.Control) cmdSave.PerformClick();
            if (e.KeyCode == Keys.F3) cmdGetData.PerformClick();
        }

        private void grdLIS_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// hàm thực hiện việc lưu lại thông tin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, EventArgs e)
        {
            foreach (Janus.Windows.GridEX.GridEXRow gridExRow in grdLIS.GetDataRows())
            {

                
            }
        }
        /// <summary>
        /// hàm thực hiện việc lấy lại thong tin của dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdGetData_Click(object sender, EventArgs e)
        {
            GetData();
        }
        /// <summary>
        /// hàm thực hiện việc lấy thông tin dữ liệu
        /// </summary>
        void GetData()
        {
            m_dtDataControl = new KetNoiService().SearchDataParaInDatacontrol(txtAlias_Name.Text,
                                                                              Utility.Int32Dbnull(
                                                                                  cboThietBi.SelectedValue, -1));
            m_dtDataDichVu =
                new KetNoiService().SearchDataInDV(GetTestType_Name(Utility.Int32Dbnull(cboThietBi.SelectedValue, -1)),
                                                   txtAlias_Name.Text);
            new KetNoiService().UpdateMapTestTypeList(ref  m_dtDataDichVu);
            grdHIS.DataSource = m_dtDataDichVu;
            grdLIS.DataSource = m_dtDataControl;

        }
        /// <summary>
        /// hàm thực hiện lấy thông tin loại xét nghiệm
        /// </summary>
        /// <param name="ThietBi_ID"></param>
        /// <returns></returns>
        private string GetTestType_Name(int ThietBi_ID)
        {
            string MaDV = "FUNNY";
            SqlQuery sqlQuery = new Select().From(DDeviceList.Schema)
                .Where(DDeviceList.Columns.DeviceId).IsEqualTo(ThietBi_ID);
            DDeviceList objDeviceList = sqlQuery.ExecuteSingle<DDeviceList>();
            if(objDeviceList!=null)
            {
                SqlQuery query =
                    new Select().From(TTestTypeList.Schema).Where(TTestTypeList.Columns.TestTypeId).IsEqualTo(
                        objDeviceList.TestTypeId);
                TTestTypeList testTypeList = query.ExecuteSingle<TTestTypeList>();
                if(testTypeList!=null)
                {
                    MaDV = testTypeList.Abbreviation;
                }

            }
            return MaDV;
        }
        /// <summary>
        /// hàm thwucj hiện việc load thông tin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_MapingPara_Load(object sender, EventArgs e)
        {
            InitalDataWhenLoad();
        }
        private void InitalDataWhenLoad()
        {
            DataBinding.BindDataCombox(cboThietBi,new KetNoiService().GetThietBi(),DDeviceList.Columns.DeviceId,DDeviceList.Columns.DeviceName,"---Chọn thiết bị---");
        }
    }
}
