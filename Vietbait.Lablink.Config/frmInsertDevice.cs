using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Janus.Windows.GridEX;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Config
{
    public partial class frmInsertDevice : Office2007Form
    {
        #region Fields

        public enum action
        {
            Insert = 0,
            Update = 1
        }

        public List<Device> AList;

        public Device Device;
        public List<Device> Devices;
        public action FormAction;

        #endregion

        #region Contructor

        public frmInsertDevice()
        {
            InitializeComponent();
        }

        #endregion

        #region Method

        private void LoadComboBox()
        {
            var stopBits = (from s in Enum.GetNames(typeof (StopBits))
                select new {display = s, value = (StopBits) Enum.Parse(typeof (StopBits), s, true)}).ToList();
            cboStopbits.DataSource = stopBits;
            cboStopbits.DisplayMember = "display";
            cboStopbits.ValueMember = "value";

            var deviceType = (from s in Enum.GetNames(typeof (ConnectorType))
                select
                    new {display = s, value = (ConnectorType) Enum.Parse(typeof (ConnectorType), s, true)})
                .ToList();
            cboDeviceType.DataSource = deviceType;
            cboDeviceType.DisplayMember = "display";
            cboDeviceType.ValueMember = "value";

            var parity = (from s in Enum.GetNames(typeof (Parity))
                select new {display = s, value = (Parity) Enum.Parse(typeof (Parity), s, true)}).ToList();
            cboParity.DataSource = parity;
            cboParity.DisplayMember = "display";
            cboParity.ValueMember = "value";

            var handshake = (from s in Enum.GetNames(typeof (Handshake))
                select new {display = s, value = (Handshake) Enum.Parse(typeof (Handshake), s, true)}).
                ToList();
            cboHandshake.DataSource = handshake;
            cboHandshake.DisplayMember = "display";
            cboHandshake.ValueMember = "value";

            if (File.Exists(Device.DeviceAsembly))
            {
                var namespaces = (from d in LoadAllDeviceFromDll(Device.DeviceAsembly)
                    select new {display = d, value = d}).ToList();
                cboNameSpace.DataSource = namespaces;
                cboNameSpace.DisplayMember = "display";
                cboNameSpace.ValueMember = "value";
            }
        }

        private void BindData()
        {
            try
            {
                //cboStopbits2.SelectedValue
                UI.TryToSetBindData(cboDeviceType, "SelectedValue", Device, "DeviceType");
                UI.TryToSetBindData(txtDeviceName, "Text", Device, "DeviceName");
                UI.TryToSetBindData(txtDeviceAsembly, "Text", Device, "DeviceAsembly");
                UI.TryToSetBindData(cboNameSpace, "SelectedValue", Device, "DeviceNameSpace");
                UI.TryToSetBindData(txtConnector, "Text", Device, "Connector");
                UI.TryToSetBindData(txtBaudRate, "Text", Device, "BaudRate");
                UI.TryToSetBindData(txtDataBits, "Text", Device, "DataBits");
                UI.TryToSetBindData(checkStatus, "Checked", Device, "Status");
                UI.TryToSetBindData(checkDTREnable, "Checked", Device, "DtrEnable");
                UI.TryToSetBindData(checkRTSEnable, "Checked", Device, "RtsEnable");
                UI.TryToSetBindData(cboParity, "SelectedValue", Device, "Parity");
                UI.TryToSetBindData(cboStopbits, "SelectedValue", Device, "StopBits");
                UI.TryToSetBindData(cboHandshake, "SelectedValue", Device, "Handshake");
                UI.TryToSetBindData(txtTimeIntelval, "Text", Device, "TimeInterval");
            }
            catch (Exception exception)
            {
                MessageBox.Show("BinData không thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidData()
        {
            try
            {
                if (string.IsNullOrEmpty(txtDeviceName.Text.Trim()))
                {
                    MessageBox.Show("Bạn chưa nhập DeviceName", "Thông Báo", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtDeviceName.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtDeviceAsembly.Text.Trim()))
                {
                    MessageBox.Show("Bạn chưa nhập DeviceAsembly", "Thông Báo", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    btnLoadDll.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtConnector.Text.Trim()))
                {
                    MessageBox.Show("Bạn chưa nhập Connector", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConnector.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtBaudRate.Text.Trim()))
                {
                    MessageBox.Show("Bạn chưa nhập BaudRate ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBaudRate.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtTimeIntelval.Text.Trim()))
                {
                    MessageBox.Show("Bạn chưa nhập TimeIntelval ", "Thông Báo", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtTimeIntelval.Focus();
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Thêm không thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return true;
        }

        private IEnumerable<string> LoadAllDeviceFromDll(string dllFile)
        {
            try
            {
                string[] result = (from t in Assembly.LoadFrom(dllFile).GetTypes()
                    where
                        (t.IsClass && !t.FullName.ToUpper().Contains("TEST") &&
                         !t.FullName.ToUpper().Contains("RECORD")) &&
                        t.FullName.ToUpper().StartsWith("VIETBAIT.LABLINK.DEVICES")
                    select t.FullName).ToArray();

                Array.Sort(result);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Event 

        private void FrmInsertDeviceLoad(object sender, EventArgs e)
        {
            LoadComboBox();
            BindData();
            if (FormAction.Equals(action.Update))
            {
                chkThemLienTuc.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidData())
                {
                    return;
                }
                switch (FormAction)
                {
                    case action.Insert:

                        if (!chkThemLienTuc.Checked)
                        {
                            Devices.Add(Device);
                            Close();
                        }
                        else
                        {
                            Devices.Add(Device);
                            checkStatus.Focus();
                            Device = new Device();
                            BindData();
                        }
                        break;
                    case action.Update:
                        Close();
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Thêm không thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void BinGrid(GridEX gridEx)
        {
            List<Device> mang = Devices.ToList();
            var FilenameList = new BindingList<Device>(mang);
            gridEx.DataSource = FilenameList;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnLoadDll_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {Filter = @"dll files (VietbaIT Device)|Vietbait.Lablink.Devices*.dll"};
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            txtDeviceAsembly.Text = Path.GetFileName(openFileDialog.FileName);
            //cboNameSpace.DataSource = LoadAllDeviceFromDll(openFileDialog.FileName);
            var namespaces = (from d in LoadAllDeviceFromDll(openFileDialog.FileName)
                select new {display = d, value = d}).ToList();
            cboNameSpace.DataSource = namespaces;
            cboNameSpace.DisplayMember = "display";
            cboNameSpace.ValueMember = "value";
        }

        #endregion
    }
}