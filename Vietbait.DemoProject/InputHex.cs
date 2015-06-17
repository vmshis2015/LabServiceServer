using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vietbait.DemoProject
{
    public partial class InputHex : Form
    {
        public string ReturnString;

        public InputHex()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData != null && iData.GetDataPresent(DataFormats.Text))
            {
                txtInput.Text = (String) iData.GetData(DataFormats.Text);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                txtInput.Text = txtInput.Text.Trim();
                var sb = new StringBuilder();
                for (int i = 0; i < txtInput.Text.Length - 1; i += 2)
                {
                    //sb.Append(
                    //    Convert.ToChar(Convert.ToUInt64(txtInput.Text.Substring(i, 2), 16)));
                    //sb.Append(
                    //    Convert.ToChar(HexLiteral2Unsigned(txtInput.Text.Substring(i, 2))));
                    //var unicode = int.Parse(txtInput.Text.Substring(i, 2), NumberStyles.HexNumber);
                }
                ReturnString = sb.ToString();
                byte[] dBytes = Enumerable.Range(0, txtInput.Text.Length)
                    .Where(x => x%2 == 0)
                    .Select(x => Convert.ToByte(txtInput.Text.Substring(x, 2), 16))
                    .ToArray();
                ReturnString = Encoding.UTF8.GetString(dBytes);

                Close();
            }
            catch (Exception)
            {
            }
        }

        public ulong HexLiteral2Unsigned(string hex)
        {
            if (string.IsNullOrEmpty(hex)) throw new ArgumentException("hex");

            int i = hex.Length > 1 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X') ? 2 : 0;
            ulong value = 0;

            while (i < hex.Length)
            {
                uint x = hex[i++];

                if (x >= '0' && x <= '9') x = x - '0';
                else if (x >= 'A' && x <= 'F') x = (x - 'A') + 10;
                else if (x >= 'a' && x <= 'f') x = (x - 'a') + 10;
                else throw new ArgumentOutOfRangeException("hex");

                value = 16*value + x;
            }

            return value;
        }
    }
}