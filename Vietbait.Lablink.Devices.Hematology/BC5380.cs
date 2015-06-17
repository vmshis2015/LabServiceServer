using System;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class BC5380 : TcpIpDevice
    {
        #region Overrides of TcpIpDevice

        public override void ProcessRawData()
        {
            try
            {
                Log.Debug("Begin Process Data");
                Log.Debug("RawData: {0}{1}{0}", DeviceHelper.CRLF, StringData);
                string[] strResult = DeviceHelper.DeleteAllBlankLine(StringData, DeviceHelper.CR);
                foreach (string result in strResult)
                {
                    //Bắt đầu bằng OBR: => xử lý ngày tháng năm 
                    if (result.StartsWith("OBR"))
                    {
                        string[] tempString = result.Split('|');
                        if (tempString.Length > 7)
                        {
                            string tempTestDate = tempString[7];
                            TestResult.TestDate = string.Format("{0}/{1}/{2}", tempTestDate.Substring(6, 2),
                                tempTestDate.Substring(4, 2), tempTestDate.Substring(0, 4));
                            TestResult.Barcode = tempString[3].Trim();
                        }
                    }
                    else if (result.StartsWith("OBX"))
                    {
                        string[] tempString = result.Split('|');
                        int resultIndex = Convert.ToInt32(tempString[1]);
                        if ((resultIndex >= 5) && (resultIndex != 16) && (resultIndex != 17) && (resultIndex != 18) &&
                            (resultIndex != 19) && (resultIndex <= 31))
                        {
                            string testName = tempString[3].Split('^')[1];
                            string testValue = tempString[5].Trim();
                            string testUnit = tempString[6].Trim();
                            string testNormalLevel = tempString[7].Trim();
                            AddResult(new ResultItem(testName, testValue, testUnit, testNormalLevel, testNormalLevel));
                            Log.Debug(
                                "Add Result Success: Testname = {0}, TestValue={1}, TestUnit={2}, TestNormalLevel={3}",
                                testName, testValue, testUnit, testNormalLevel);
                        }
                    }
                }

                Log.Debug("Begin Import Result");
                ImportResults();
                Log.Debug("Import Result Success");
            }
            catch (Exception ex)
            {
                Log.Error("Error While process Data {0} {1}", DeviceHelper.CRLF, ex.ToString());
            }
            finally
            {
                ClearData();
            }
        }

        #endregion
    }
}