using System;
using System.Text.RegularExpressions;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Urine
{
    internal class ComboStikR700 : Rs232Base
    {       

        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin Process Data");
                Log.Debug("Raw Data:\n{0}", StringData);
                
                string[] arrPatients = SeparatorData(StringData);
                Log.Trace("Result has {0} Patients", arrPatients.Length);

                //Duyệt từng bệnh nhân để xử lý dữ liệu
                foreach (string record in arrPatients)
                {
                    try
                    {
                        string strInput = record;

                        string[] strResult = strInput.Split(DeviceHelper.CR);

                        //Nếu dữ liệu nhỏ hơn 12 dòng là không hợp lệ
                        if (strResult.Length < 12) continue;
                        //Tìm đến index của GLU
                        int idBld = -1;
                        for (int i = 0; i < strResult.Length; i++)
                            if (strResult[i].StartsWith("BLD")) idBld = i;

                        //Nếu không tìm thấy phần tử GLU nào thì bỏ qua.
                        if (idBld == -1) continue;

                        //Nếu tìm thấy GLU nhưng độ dài chuỗi không đủ => bỏ qua
                        if (strResult.Length < (idBld + 10)) continue;

                        // Bắt đầu xử lý dữ liệu
                        TestResult.TestSequence = strResult[0];
                        TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");

                        for (int i = idBld; i <= idBld+10; i++)
                        {
                            //var tempResult = DeviceHelper.DeleteAllBlankLine(strResult[i], " ", true);
                            //string testName = tempResult[0];
                            //string testValue = tempResult[tempResult.Length - 1];
                            AddResult(GetResultItemFromString(strResult[i]));
                        }

                        // Tìm barcode
                        for (int i = 0; i < strResult.Length; i++)
                            if (strResult[i].StartsWith("ID(")) idBld = i;

                        TestResult.Barcode = strResult[idBld].Replace("ID(", "").Replace(")", "").Trim();
                        Log.Debug("Barcode:{0}, TestDate:{1}", TestResult.Barcode, TestResult.TestDate);

                        Log.Debug(string.Format("Import Result For barocde:{0}", TestResult.Barcode));
                        ImportResults();
                        Log.Debug(string.Format("Import Result Success"));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error while processing data {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing data {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                throw;
            }
            finally
            {
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
                s = s.Substring(12).Trim();
                var re = new Regex(@"[-+]?([0-9]*\.[0-9]+|[0-9]+)");
                Match m = re.Match(s);
                if (m.Success)
                {
                    testValue = m.Value;
                    testUnit = s.Substring(testValue.Length + m.Index);
                }
                else
                {
                    testValue = s;
                }
                if (testValue.ToUpper().Contains("NEG")) testValue = "Âm Tính";
                else if (testValue.ToUpper().Contains("POS")) testValue = "Dương Tính";
                return new ResultItem(testName, testValue, testUnit);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string[] SeparatorData(string stringData)
        {
            try
            {
                //Bỏ tất cả các ký tự cho đến khi gặp ký tự "P" đầu tiên
                int indexOfName = stringData.IndexOf("Name:");
                if(indexOfName<0)return new string[]{};
                stringData = stringData.Substring(indexOfName);

                //Ngắt chuỗi theo ký tự CR
                string[] arrStringData = DeviceHelper.DeleteAllBlankLine(stringData, DeviceHelper.CRLF, true);

                //Biến để lưu tất cả các chuỗi kết quả.
                //Mỗi phần tử của chuỗi là một kết quả của bệnh nhân gồm các record P,O,R,C.....
                var allResult = new string[] { };

                //Biến lưu Index
                int id = 0;

                foreach (string line in arrStringData)
                {
                    if (line.StartsWith("Name:"))
                    {
                        id++;
                        Array.Resize(ref allResult, id);
                    }
                    allResult[id - 1] = allResult[id - 1] + line + DeviceHelper.CR;
                }

                return allResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}