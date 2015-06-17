using System;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Vesmatec20 : Rs232Base
    {
        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin process Data:{0}", StringData);

                // Ngắt toàn bộ chuỗi kết quả
                string[] strResutl = DeviceHelper.DeleteAllBlankLine(StringData, DeviceHelper.CRLF, false);

                int numId = -1;
                // Tìm dòng bắt đầu bằng chữ NUM:
                for (int index = 0; index < strResutl.Length; index++)
                {
                    if (!strResutl[index].ToUpper().StartsWith("NUM")) continue;
                    numId = index;
                    break;
                }

                for (int i = numId + 1; i < strResutl.Length; i++)
                {
                    try
                    {
                        // Kết quả của một bệnh nhân
                        string p = strResutl[i];
                        // Nếu không có dữ liệu dừng luôn
                        if (p.ToUpper().Contains("SAMPLE")) continue;

                        // Nếu có dữ liệu bắt đầu xử lý
                        //// Lấy ra chuỗi kết quả
                        string allResult = p.Substring(0, 24);
                        string allId = p.Substring(24);

                        //// Xử lý Barcode
                        TestResult.Barcode = !allId.Contains("=")
                            ? ""
                            : allId.Substring(allId.IndexOf('=') + 1).Replace(".", "").Trim();

                        //// Xử lý từng kết quả:
                        string[] results =
                            DeviceHelper.DeleteAllBlankLine(allResult.Substring(allResult.IndexOf('=') + 1), " ", true);
                        for (int idx = 0; idx < results.Length; idx++)
                        {
                            string testName = "";
                            if (idx == 0) testName = "1h";
                            else if (idx == 1) testName = "2h";
                            else if (idx == 2) testName = "Ind";

                            AddResult(new ResultItem(testName, results[idx]));
                        }
                        Log.Debug("Begin Import Result for barcode:{0}", TestResult.Barcode);
                        Log.Debug(ImportResults() ? "Insert Result success" : "Insert result false");
                    }
                    catch (Exception)
                    {
                    }
                }
                ClearData();
            }
            catch (Exception ex)
            {
                Log.Error("Error while process data:ERR-{0}", ex);
                ClearData();
            }
        }
    }
}