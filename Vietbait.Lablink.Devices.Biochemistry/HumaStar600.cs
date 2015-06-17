using System;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    internal class HumaStar600 : Rs232AstmDevice
    {
        /// <summary>
        ///     Xử lý sau kkhi nhận được dữ liệu
        /// </summary>
        public override void ProcessData()
        {
            try
            {
                Log.Trace("Begin Process Data");
                Log.Trace(DeviceHelper.CRLF + StringData);

                //Lấy về dữ liệu của các bệnh nhân
                string[] arrPatients = SeparatorData(StringData);

                Log.Trace("Result has {0} Patients", arrPatients.Length);
                //Duyệt qua mảng xử lý dữ liệu của từng bệnh nhân
                foreach (string patient in arrPatients)
                {
                    foreach (string record in patient.Split(DeviceHelper.CR))
                    {
                        string[] temp;
                        if (record.StartsWith("P"))
                            try
                            {
                                temp = record.Split('|');
                                TestResult.Barcode = temp[2].Trim();
                                Log.Debug("Barcode:{0}", TestResult.Barcode);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while parse data: {0}{1}Eror String:{2}", record, DeviceHelper.CRLF,
                                    ex.ToString());
                            }
                        else if (record.StartsWith("R"))
                            try
                            {
                                temp = record.Split('|');
                                string tempDate = temp[12];
                                if (string.IsNullOrEmpty(TestResult.TestDate))
                                {
                                    TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate.Substring(6, 2),
                                    tempDate.Substring(4, 2),
                                    tempDate.Substring(0, 4));
                                }

                                string testname = temp[2].Split('^')[3].Trim();
                                string testvalue = temp[3].Trim();
                                string testunit = temp[4].Trim();
                                AddResult(new ResultItem(testname,testvalue,testunit));
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while parse data: {0}{1}Eror String:{2}", record, DeviceHelper.CRLF,
                                    ex.ToString());
                            }
                    }

                    Log.Debug(string.Format("Import Result For barocde"));
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