using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryToolCheckSumvalue
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        public const char NULL = (char)0;
        public const char STX = (char)2;
        public const char ETX = (char)3;
        public const char EOT = (char)4;
        public const char ENQ = (char)5;
        public const char ACK = (char)6;
        public const char CR = (char)13;
        public const char LF = (char)10;
        public const char VT = (char)11;
        public const char NAK = (char)21;
        public const char ETB = (char)23;
        public const char FS = (char)28;
        public const char GS = (char)29;
        public const char RS = (char)30;
        public const char SOH = (char)1;
        public const char SYN = (char)22;
        public const char DC1 = (char)17;
        public const char DC2 = (char)18;
        public const char DC3 = (char)19;
        public const char DC4 = (char)20;
        public const string MainServiceLogger = "_LABLink Service";
        public static int REPORTTYPE;
        public static readonly string CRLF = String.Format("{0}{1}", CR, LF);
        /// <summary>
        ///     Hàm trả về chuỗi Checksum của một Frame.
        ///     Check khi có một trong hai giá trị ETX, ETB
        /// </summary>
        /// <param name="frame">Chuỗi cần kiểm tra</param>
        /// <returns>Chuỗi trả về là 2 ký tự dùng để checksum</returns>
       
         public static string GetCheckSumValue(string frame)
        {
            string checksum = "00";

            int sumOfChars = 0;
            bool complete = false;

            //take each byte in the string and add the values
            for (int idx = 0; idx < frame.Length; idx++)
            {
                int byteVal = Convert.ToInt32(frame[idx]);

                switch (byteVal)
                {
                    case STX:
                        sumOfChars = 0;
                        break;
                    case ETX:
                    case ETB:
                        sumOfChars += byteVal;
                        complete = true;
                        break;
                    default:
                        sumOfChars += byteVal;
                        break;
                }

                if (complete)
                    break;
            }

            if (sumOfChars > 0)
            {
                //hex value mod 256 is checksum, return as hex value in upper case
                checksum = Convert.ToString(sumOfChars % 256, 16).ToUpper();
            }

            //if checksum is only 1 char then prepend a 0
            return (checksum.Length == 1 ? "0" + checksum : checksum);
        }

        private void btnChecksum_Click(object sender, EventArgs e)
        {
            GetCheckSumValue(txtChecksum.Text.Trim());

            txtResult.Text = GetCheckSumValue(txtChecksum.Text.Trim());
        }


    }
}
