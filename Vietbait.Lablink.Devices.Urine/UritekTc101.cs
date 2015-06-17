using System;
using System.Text.RegularExpressions;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Urine
{
    internal class UritekTc101 : Rs232Base
    {
        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin Process Data");
                Log.Trace("Raw Data:\r\n{0}", StringData);
                if (!StringData.EndsWith(DeviceHelper.ETX.ToString())) return;
                // Lấy ra kết quả của bệnh nhân 
                string[] allResultPatients = DeviceHelper.SeperatorRawData(StringData, DeviceHelper.STX,
                    DeviceHelper.ETX);
                Log.Trace("Result has {0} Patients", allResultPatients.Length);

                // Xử lý từng kết quả của bệnh nhân.
                foreach (string patient in allResultPatients)
                {
                    // Loại bỏ những dấu * ở đầu dòng và ngắt dòng theo ký tự CRLF 
                    string[] result = DeviceHelper.DeleteAllBlankLine(patient, DeviceHelper.CRLF, true);
                    // Lấy ra PatientID
                    int rowIndex = GetRowIndex(result, "NO");
                    TestResult.Barcode = result[rowIndex].Split()[2].Trim();
                    string[] tempDate = result[rowIndex + 1].Split()[0].Split('-');
                    // Lấy ngày 
                    TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate[2].Trim(), tempDate[1].Trim(),
                        tempDate[0].Substring(4, 4).Trim());
                    rowIndex = GetFirstResultIndex(result);
                    // Lọc kết quả và xử lý kết quả theo bệnh nhân.
                    for (int i = rowIndex; i < result.Length - 1; i++)
                    {
                        AddResult(GetResultItemFromString(result[i]));
                    }
                    Log.Debug("Begin Import Result For barocde:{0}", TestResult.Barcode);
                    Log.Trace(ImportResults() ? "Import Result Success" : "Error Import Result");
                }
                Log.Trace("Finish Process Data");
                ClearData();
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing data: {0}", ex.ToString());
                ClearData();
            }
        }

        private ResultItem GetResultItemFromString(string s)
        {
            try
            {
                string testName = s.Substring(0, 4).Replace("*", "").Trim();
                string testValue;
                string testUnit = string.Empty;
                var re = new Regex(@"[-+]?([0-9]*\.[0-9]+|[0-9]+)");
                Match m = re.Match(s);
                if (m.Success)
                {
                    testValue = m.Value;
                    testUnit = s.Substring(testValue.Length + m.Index);
                }
                else
                {
                    testValue = s.Substring(4).Trim();
                }
                return new ResultItem(testName, testValue, testUnit);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Trả về index của dòng với chuỗi bắt đầu xác định
        /// </summary>
        /// <param name="arrString">Mảng cần tìm Index</param>
        /// <param name="beginString">Chuỗi bắt đầu</param>
        /// <returns></returns>
        private int GetRowIndex(string[] arrString, string beginString)
        {
            try
            {
                int result = -1;
                for (int i = 0; i < arrString.Length; i++)
                    if (arrString[i].StartsWith(beginString))
                    {
                        result = i;
                        break;
                    }
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        ///     Trả về index của dòng với chuỗi bắt đầu xác định
        /// </summary>
        /// <param name="arrString">Mảng cần tìm Index</param>
        /// <param name="beginString">Chuỗi bắt đầu</param>
        /// <returns></returns>
        private int GetFirstResultIndex(string[] arrString)
        {
            try
            {
                int result = -1;
                for (int i = 0; i < arrString.Length; i++)
                    if (arrString[i].ToUpper().Contains("GLU"))
                    {
                        result = i;
                        break;
                    }
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}