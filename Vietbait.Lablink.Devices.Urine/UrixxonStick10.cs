using System;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Urine
{
    internal class UrixxonStick10 : Rs232Base
    {
        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin Process Data");
                Log.Trace(DeviceHelper.CRLF + StringData);
                string[] allPatients = DeviceHelper.SeperatorRawData(StringData, DeviceHelper.STX, DeviceHelper.ETX);

                foreach (string patient in allPatients)
                {
                    try
                    {
                        string[] tempResult = DeviceHelper.DeleteAllBlankLine(patient, DeviceHelper.CRLF, false);
                        string[] tempTestDate = tempResult[4].Split()[0].Split('.');
                        TestResult.TestDate = string.Format("{0}/{1}/{2}", tempTestDate[2], tempTestDate[1],
                            tempTestDate[0]);
                        try
                        {
                            string tempbarcode = tempResult[3].Split(':')[1].Trim();
                            //if(string.IsNullOrEmpty(tempbarcode))
                            //    tempbarcode = tempResult[2].Split(':')[1].Trim().Replace(".","");
                            TestResult.Barcode = tempbarcode == "" ? "0000" : tempbarcode;
                        }
                        catch (Exception)
                        {
                            TestResult.Barcode = "0000";
                        }

                        for (int i = 6; i < 16; i++)
                        {
                            try
                            {
                                string[] tempresultarr = DeviceHelper.DeleteAllBlankLine(tempResult[i].Substring(2), "");
                                if (tempresultarr.Length > 1)
                                {
                                    string testName = tempresultarr[0].Replace("*", "");
                                    string testValue = tempresultarr[1];
                                    TestResult.Add(new ResultItem(testName, testValue));
                                    Log.Debug("Add new Result: TestName = {0}, TestValue = {1}", testName, testValue);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        Log.Debug("Begin Import Result");
                        Log.Debug(ImportResults() ? "Import Result Success" : "Error while import result");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
                ClearData();
                throw;
            }
            finally
            {
                ClearData();
            }
        }
    }
}