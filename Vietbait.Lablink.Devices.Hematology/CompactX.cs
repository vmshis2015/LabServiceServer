using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    public class CompactX : Rs232Base
    {
        /// <summary>
        /// Xử lý sau kkhi nhận được dữ liệu
        /// </summary>
        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin Process Data");
                Log.Trace(string.Format("{0}{1}", DeviceHelper.CRLF, StringData));
                
                var valid = new List<char> {'1', '3', '5', '7', '9'};
                //Lấy về dữ liệu của các bệnh nhân
                string[] arrResult = DeviceHelper.DeleteAllBlankLine(StringData,DeviceHelper.CRLF,false);
                //string[] arrResult = StringData.Split(DeviceHelper.GS);
                //string[] arrPatients = (from a in arrPatients1 where valid.Contains(a[0]) select a).ToArray();
                
                var s = arrResult[arrResult.Length-1];
                if(!s.StartsWith(DeviceHelper.GS.ToString()))return;
                //if(!s.EndsWith(DeviceHelper.CRLF))return;

                //Biến lưu Index
                //int id = 0;

                //var patient = new string[] {};
                //foreach (string line in arrResult)
                //{
                //    if (line.StartsWith(DeviceHelper.RS.ToString()))
                //    {
                //        id++;
                //        Array.Resize(ref patient, id);
                //        patient[id - 1] = patient[id - 1] + line;
                //    }
                //}

                // lấy về dữ liệu của từng bệnh nhân
                foreach (string r in arrResult)
                {
                    if(r.StartsWith(DeviceHelper.GS.ToString()))
                    {
                        Log.Debug(string.Format("Import Result For barocde:{0}", TestResult.Barcode));
                        Log.Debug(ImportResults() ? "Import Result Success" : "Error while import result");
                        continue;
                    }
                    if (!r.StartsWith(DeviceHelper.RS.ToString())) continue;
                    if(!valid.Contains(r[1]))continue;
                    string[] temp = r.Split('|');
                    TestResult.Barcode = temp[3].Trim();
                    TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                    string testName = temp[4].Trim();
                    string testValue = string.Format("{0}", temp[10].Trim());
                    //string testUnit = temp[10].Trim();
                    AddResult(new ResultItem(testName, testValue));
                    Log.Debug("Add Result Item success: TestName={0}, TestValue={1}", testName,
                              testValue);

                    testName = string.Format("{0}TG", testName);
                    testValue = temp[9].Trim();
                    AddResult(new ResultItem(testName, testValue));
                    Log.Debug("Add Result Item success: TestName={0}, TestValue={1}", testName,
                              testValue);
                    testName = temp[4].Trim();

                    if (testName.Equals("0"))
                    {
                        testName = string.Format("{0}IRN", testName);
                        testValue = temp[11].Trim();
                        AddResult(new ResultItem(testName, testValue));
                        Log.Debug("Add Result Item success: TestName={0}, TestValue={1}", testName,
                                  testValue);
                    }
                }
                ClearData();
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing data {0}Error:{1}", DeviceHelper.CRLF, ex.ToString());
            }
        }
    }
}