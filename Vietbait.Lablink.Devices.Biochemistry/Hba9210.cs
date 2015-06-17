using System;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    internal class Hba9210 : Rs232Base
    {
        /// <summary>
        ///     Xử lý sau kkhi nhận được dữ liệu
        /// </summary>
        public override void ProcessRawData()
        {
            try
            {
                // Kiểm tra dữ liệu xem có đủ không ?
                if (!StringData.EndsWith(DeviceHelper.ETX.ToString()))return;


                Log.Trace("Begin Process Data");
                Log.Trace(DeviceHelper.CRLF + StringData);

                //Lấy về dữ liệu của các bệnh nhân
                string[] arrPatients = DeviceHelper.SeperatorRawData(StringData, DeviceHelper.STX, DeviceHelper.ETX);

                Log.Trace("Result has {0} Patients", arrPatients.Length);
                //Duyệt qua mảng xử lý dữ liệu của từng bệnh nhân
                foreach (string patient in arrPatients)
                {
                    var rawdata = patient.Split(',');
                    var tempDate = rawdata[4].Substring(1).Split('-');
                    TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate[2], tempDate[1], tempDate[0]);
                    string barcode = rawdata[10].Substring(1).Trim();
                    TestResult.Barcode = barcode;
                    string testValue = rawdata[11].Trim();
                    try
                    {
                        testValue = Convert.ToDouble(testValue).ToString();
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                    TestResult.Add(new ResultItem("HbA1C", testValue));
                    Log.Debug(string.Format("Import Result For barocde:{0}", barcode));
                    Log.Debug(ImportResults()
                        ? string.Format("Import Result Success")
                        : string.Format("Error while import result"));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing data {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
            }
            finally
            {
                ClearData();
            }
        }

        private string[] SeparatorData(string stringData)
        {
            try
            {
                //Bỏ tất cả các ký tự cho đến khi gặp ký tự "P" đầu tiên
                stringData = stringData.Substring(stringData.IndexOf("P|1"));

                //Ngắt chuỗi theo ký tự CR
                string[] arrStringData = stringData.Split(DeviceHelper.CR);

                //Biến để lưu tất cả các chuỗi kết quả.
                //Mỗi phần tử của chuỗi là một kết quả của bệnh nhân gồm các record P,O,R,C.....
                var allResult = new string[] {};

                //Biến lưu Index
                int id = 0;
                foreach (string line in arrStringData)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("P"))
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
                Log.Error("Error while parsing data {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                throw;
            }
        }
    }
}