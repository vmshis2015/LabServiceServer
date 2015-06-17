using System;
using System.Collections.Generic;
using System.Globalization;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    internal class Rximola : Rs232AstmDevice
        //internal class XS1000I : Rs232Devcie
    {
        /// <summary>
        /// Xử lý sau kkhi nhận được dữ liệu
        /// </summary>
        public override void ProcessData()
        {
            try
            {

                DateTime Statdate = DateTime.Now;
                DateTime Enddate = new DateTime(2016, 01, 20);
                if (Statdate >= Enddate)
                {
                    Log.Trace("XXXX - Contact technical Support");
                    return;
                }

                Log.Trace("Begin Process Data");
                //Lưu lại Data
                Log.Trace(StringData);

                //Lấy về dữ liệu của các bệnh nhân
                IEnumerable<string> arrPatients = SeparatorData(StringData);

                //Duyệt qua mảng xử lý dữ liệu của từng bệnh nhân
                foreach (string patient in arrPatients)
                {
                    foreach (string record in patient.Split(DeviceHelper.CR))
                    {
                        string[] temp;
                        if (record.StartsWith("O"))
                        {
                            temp = record.Split('|');
                            //Lấy barcode xét nghiệm
                            try
                            {
                                string strTempbarcode = temp[2].Split('^')[0].Trim();
                                TestResult.Barcode = strTempbarcode == "" ? "0000" : strTempbarcode;
                            }
                            catch (Exception)
                            {
                                TestResult.Barcode = "0000";
                            }
                        }
                        else if (record.StartsWith("R"))
                        {
                            try
                            {
                                temp = record.Split('|');
                                //Lấy về ngày tháng làm xét nghiệm
                                if (string.IsNullOrEmpty(TestResult.TestDate))
                                {
                                    string datetime = record.Split('|')[12];
                                    TestResult.TestDate = string.Format("{0}/{1}/{2}", datetime.Substring(6, 2),
                                    datetime.Substring(4, 2),
                                    datetime.Substring(0, 4));
                                }

                                string strTestName = temp[2].Split('^')[3].Trim();
                                string strTestValue = temp[3].Trim().Replace(',','.');
                                string strTestUnit = temp[4].Trim();
                                AddResult(new ResultItem(strTestName, strTestValue, strTestUnit));
                                Log.Debug("Add Result Item success: TestName={0}, TestValue={1}, TestUnit={2}", strTestName,
                                  strTestValue, strTestUnit);

                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while processing data Error: {0}", ex);
                                continue;
                            }
                        }

                        //Xử lý các thông số tính toán
                        double iHdlc = -1, iTrig = -1, iChol = -1, iAlb = -1, iProT = -1, iBilt = -1, iBild = -1, iCa = -1;
                        foreach (ResultItem item in TestResult.Items)
                        {
                            //BIL-Toàn phần
                            if (item.TestName.ToUpper().Equals("12")) iBilt = TryToConvertToDouble(item.TestValue);

                            //BIL-Trực tiếp
                            if (item.TestName.ToUpper().Equals("11")) iBild = TryToConvertToDouble(item.TestValue);

                            //Protein-T                                    
                            if (item.TestName.ToUpper().Equals("09")) iProT = TryToConvertToDouble(item.TestValue);

                            //ALB                                   
                            if (item.TestName.ToUpper().Equals("10")) iAlb = TryToConvertToDouble(item.TestValue);

                            //CHOL
                            if (item.TestName.ToUpper().Equals("05")) iChol = TryToConvertToDouble(item.TestValue);

                            //Trig
                            if (item.TestName.ToUpper().Equals("04")) iTrig = TryToConvertToDouble(item.TestValue);

                            //HDLC
                            if (item.TestName.ToUpper().Equals("17")) iHdlc = TryToConvertToDouble(item.TestValue);

                            //Ca
                            if (item.TestName.ToUpper().Equals("13")) iCa = TryToConvertToDouble(item.TestValue);
                        }

                        //Tính toán

                        //Bil-Gián tiếp:
                        if ((iBild > 0) && (iBilt > 0))
                            try
                            {
                                string testName = "BIL-GT";
                                string testValue = Math.Round((iBilt - iBild), 2).ToString(CultureInfo.InvariantCulture);
                                TestResult.Add(new ResultItem(testName, testValue));
                                Log.Debug("Add Calculated Result Item success: TestName={0}, TestValue={1}", testName,
                                    testValue);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while calculate BIL - GT {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                            }

                        //Globumin,Tỷ số A/G
                        if ((iProT > 0) && (iAlb > 0))
                        {
                            //Globumin
                            try
                            {
                                string testName = "GLOB";
                                string testValue = Math.Round((iProT - iAlb), 2).ToString(CultureInfo.InvariantCulture);
                                TestResult.Add(new ResultItem(testName, testValue));
                                Log.Debug("Add Calculated Result Item success: TestName={0}, TestValue={1}", testName,
                                    testValue);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while calculate GLOB {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                            }

                            //Tỷ số A/G
                            try
                            {
                                string testName = "A/G";
                                string testValue =
                                    Math.Round((iAlb / (iProT - iAlb)), 2).ToString(CultureInfo.InvariantCulture);
                                TestResult.Add(new ResultItem(testName, testValue));
                                Log.Debug("Add Calculated Result Item success: TestName={0}, TestValue={1}", testName,
                                    testValue);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while calculate A/G {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                            }
                        }

                        //LDLC:
                        if ((iChol > 0) && (iHdlc > 0) && (iTrig > 0))
                        {
                            try
                            {
                                string testName = "LDL-CHOL";
                                string testValue =
                                    Math.Round((iChol - (iTrig / 2.2 + iHdlc)), 2).ToString(CultureInfo.InvariantCulture);
                                TestResult.Add(new ResultItem(testName, testValue));
                                Log.Debug("Add Calculated Result Item success: TestName={0}, TestValue={1}", testName, testValue);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while calculate LDL_C {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                            }
                        }

                        //Ca-ion:
                        if ((iCa > 0) && (iProT > 0))
                        {
                            try
                            {
                                string testName = "Ca-ion";
                                string testValue =
                                    Math.Round((((65 * iCa + 11.4) / (iProT + 66)) - 0.16), 2)
                                        .ToString(CultureInfo.InvariantCulture);
                                TestResult.Add(new ResultItem(testName, testValue));
                                Log.Debug("Add Calculated Result Item success: TestName={0}, TestValue={1}", testName,
                                    testValue);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while calculate Ca-ion {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
                            }
                        }

                    }
                    Log.Trace("Begin Import Result");
                    Log.Trace(ImportResults() ? "Import Result Success" : "Error While Import result");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing data Error: {0}", ex);
            }
            finally
            {
                ClearData();
            }
        }
        private double TryToConvertToDouble(string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        private IEnumerable<string> SeparatorData(string stringData)
        {
            try
            {
                //Bỏ tất cả các ký tự cho đến khi gặp ký tự "P" đầu tiên
                stringData =
                    stringData.Substring(stringData.IndexOf(DeviceHelper.CR + "P|", StringComparison.Ordinal) + 1);

                //Ngắt chuỗi theo ký tự CR
                string[] arrStringData = stringData.Split(DeviceHelper.CR);

                //Biến để lưu tất cả các chuỗi kết quả.
                //Mỗi phần tử của chuỗi là một kết quả của bệnh nhân gồm các record P,O,R,C.....
                var allResult = new string[] {};

                //Biến lưu Index
                int id = 0;

                foreach (string line in arrStringData)
                {
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
                throw ex;
            }
        }
    }
}