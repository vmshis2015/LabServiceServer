using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Vietbait.Lablink.Interface
{
    public class BarcodeInfo
    {
        public BarcodeInfo()
        {
            BarcodeData = "";
            BarcodeData2 = "";
            InsuranceNum = "";
            Sex = 1;
            PatientName = "";
            Tag = "";
            DisplayData = true;
            BarcodeSize = new Size(2000, 1000);
            BarcodeFontSize = 190;
            BarcodeFont = new Font("Arial", BarcodeFontSize, FontStyle.Bold, GraphicsUnit.Point, 0);
            BarcodeSymbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128;
            NumberOfCopies = 1;
        }

        public string BarcodeData { get; set; }
        public string BarcodeData2 { get; set; }
        public string InsuranceNum { get; set; }
        public int Sex { get; set; }
        public string Dob { get; set; }
        public int Yearbirth { get; set; }
        public string PatientName { get; set; }
        public string Tag { get; set; }
        public bool DisplayData { get; set; }
        public Font BarcodeFont { get; set; }
        public float BarcodeFontSize { get; set; }
        public Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies BarcodeSymbology { get; set; }
        public Size BarcodeSize { get; set; }
        public int NumberOfCopies { get; set; }
        public string Department { get; set; }
        public string Room { get; set; }
        public DateTime Date { get; set; }

        public static Mabry.Windows.Forms.Barcode.Barcode CreateNewBarcode(string vBarcodeData)
        {
            try
            {
                var vBarcode = new Mabry.Windows.Forms.Barcode.Barcode();
                vBarcode.Data = vBarcodeData;
                vBarcode.Size = new Size(600, 300);
                //vBarcode.Font = new Font("Arial", 44, FontStyle.Regular, GraphicsUnit.Point, 0); 
                vBarcode.Font = new Font("Microsoft Sans Serif", 44, FontStyle.Regular, GraphicsUnit.Point, 0);
                vBarcode.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128;
                return vBarcode;
            }
            catch (Exception)
            {
                var vBarcode = new Mabry.Windows.Forms.Barcode.Barcode();
                vBarcode.Data = "0000000000";
                vBarcode.Size = new Size(600, 300);
                vBarcode.Font = new Font("Arial", 44, FontStyle.Regular, GraphicsUnit.Point, 0);
                vBarcode.Symbology = Mabry.Windows.Forms.Barcode.Barcode.BarcodeSymbologies.Code128;
                return vBarcode;
            }
        }

        public static byte[] GetByteDataOfBarcode(string vBarCodeData)
        {
            try
            {
                Mabry.Windows.Forms.Barcode.Barcode myPrivateBarcode = CreateNewBarcode(vBarCodeData);
                Bitmap img;
                var ms = new MemoryStream();
                img = myPrivateBarcode.Image();
                img.Save(ms, ImageFormat.Tiff);
                var data = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(data, 0, (int)ms.Length);
                return data;
            }
            catch (Exception)
            {
                return  new byte[]{};
            }
        }
    }
}