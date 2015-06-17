using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Vietbait.Lablink.Notifier
{
    public partial class FrmConfig : Form
    {
        public NotifierProperties Object;

        public FrmConfig()
        {
            InitializeComponent();
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            try
            {
                grdProperties.SelectedObject = Object;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình nạp form {0}", ex.ToString());
            }
        }

        private void FrmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {

            var myWriter = new StreamWriter(string.Format("{0}{1}.xml", AppDomain.CurrentDomain.BaseDirectory, Object.GetType().Name));
            try
            {
                var mySerializer = new XmlSerializer(Object.GetType());
                mySerializer.Serialize(myWriter, Object);
            }
            catch (Exception)
            {
            }
            finally
            {
                myWriter.Flush();
                myWriter.Close();
            }
        }
    }
}
