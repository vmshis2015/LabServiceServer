using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Advia2120_2 : Advia2120Device
    //internal class Advia2120 : Rs232Base
    {

        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin process Data:{0}",StringData);
                var strResult = DeviceHelper.DeleteAllBlankLine(StringData, DeviceHelper.CRLF, false);

                // Xử lý Barcode and DateTime
                var strTempBarcodeAndDateTime = DeviceHelper.DeleteAllBlankLine(strResult[0], " ");

                // Lấy Barcode
                var strTempBarcode = strTempBarcodeAndDateTime[0];
                while (strTempBarcode.StartsWith("0"))
                {
                    strTempBarcode = strTempBarcode.Substring(1);
                }
                TestResult.Barcode = strTempBarcode;
                // Lấy ngày tháng
                string[] strTempDate = strTempBarcodeAndDateTime.Length == 4
                    ? strTempBarcodeAndDateTime[2].Split('/')
                    : strTempBarcodeAndDateTime[1].Split('/');
                TestResult.TestDate = string.Format("{0}/{1}/{2}{3}", strTempDate[1], strTempDate[0],
                    DateTime.Now.Year.ToString().Substring(0, 2), strTempDate[2]);

                var rawResult = (from r in Regex.Split(strResult[1], "(.{9})")
                    where r.Trim().Length > 0
                    select new ResultItem(r.Substring(0, 3).Trim(), r.Substring(3).Trim()))
                    .OrderBy(x => Convert.ToInt32(x.TestDataId)).ToList();
                
                    AddResult(rawResult);
                

                // Import kết quả
                Log.Debug("Begin Import Result");
                Log.Debug(ImportResults() ? "Finish Import Result" : "Import Result Error");

            }
            catch (Exception)
            {
                ClearData();
                //throw;
            }
        }
    }
}