using System;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    internal class Bs380 : TcpIpDevice
    {
        #region Constant

        private const string MessageHeaderStr = "MSH";
        private const string MessageObr = "OBR";
        private const string MessageOBX = "OBX";
        private const char FieldSeparator = '|';
        private static readonly string SegmentSeparator = DeviceHelper.CRLF;

        #endregion

        public override void ProcessRawData()
        {
            try
            {
                Log.Debug("Begin Process Data");
                Log.Debug("RawData: {0}{1}{0}", DeviceHelper.CRLF, StringData);
                string[] arrAllPatient = DeviceHelper.SeperatorRawData(StringData, DeviceHelper.VT, DeviceHelper.FS);
                Log.Debug("Find {0} Patients", arrAllPatient.Length);
                foreach (string resultItem in arrAllPatient)
                {
                    try
                    {
                        string[] strResult = resultItem.Split(SegmentSeparator.ToCharArray());
                        strResult = DeviceHelper.DeleteAllBlankLine(strResult);
                        string ackTempString = string.Empty;
                        foreach (string tempString in strResult)
                        {
                            string[] tempArr = tempString.Split(FieldSeparator);
                            if (tempString.StartsWith(MessageHeaderStr))
                            {
                                ackTempString = tempString;
                                string tempTestDate = tempArr[6];
                                TestResult.TestDate = String.Format("{0}/{1}/{2}", tempTestDate.Substring(6, 2),
                                    tempTestDate.Substring(4, 2),
                                    tempTestDate.Substring(0, 4));
                            }
                            else if (tempString.StartsWith(MessageObr))
                            {
                                TestResult.Barcode = tempArr[3];
                            }
                            else if (tempString.StartsWith(MessageOBX))
                            {
                                var item = new ResultItem(tempArr[4].Trim(), tempArr[13].Trim(), tempArr[6]);
                                AddResult(item);
                            }
                        }

                        Log.Debug("Begin Import Result");
                        Log.Debug(ImportResults() ? "Import Result Success" : "Error while import result");

                        //Send Confirm cho Devcies
                        ackTempString = ackTempString.Replace("ORU^R01", "ACK^R01");
                        string sendData = string.Format("{0}{1}{2}MSA|AA|1|Message accepted|||0|{3}{4}{5}",
                            DeviceHelper.VT,
                            ackTempString, DeviceHelper.CR, DeviceHelper.CR, DeviceHelper.FS,
                            DeviceHelper.CR);

                        SendStringData(sendData);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error While processing Data:\r\n{0}\r\n{1}", resultItem, ex.ToString());
                    }
                }
                ClearData();
            }
            catch (Exception)
            {
                ClearData();
            }
        }
    }
}