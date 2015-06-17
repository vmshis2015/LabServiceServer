using System;
using System.Collections.Generic;
using Vietbait.Lablink.Utilities;
using Vietbait.Lablink.Workflow;

namespace Vietbait.Lablink.Devices.Immunology
{
    public class Cobase411TcpIpManager : TcpIpASTMManager
    {
        private CobasE411HeaderRecord _clsHRecord;
        private CobasE411TestOrderRecord _clsORecord;
        private CobasE411PatientInformationRecord _clsPRecord;
        private CobasE411RequestInformationRecord _clsQRecord;
        private CobasE411ResultRecord _clsRRecord;
        private CobasE411TerminationRecord _clsTRecord;
        private string _sQBarcode;


        public Cobase411TcpIpManager()
        {
            try
            {
                //if (DateTime.Now > new DateTime(2013, 06, 06)) throw new Exception("XXX");
                _clsHRecord = new CobasE411HeaderRecord();
                _clsPRecord = new CobasE411PatientInformationRecord();
                _clsORecord = new CobasE411TestOrderRecord();
                _clsQRecord = new CobasE411RequestInformationRecord();
                _clsRRecord = new CobasE411ResultRecord();
                _clsTRecord = new CobasE411TerminationRecord();
            }
            catch (Exception ex)
            {
                Log.Error("Fatal Error: {0}", ex);
            }
        }

        public override bool ProcessData(string inputBuffer, ref List<string> orderList)
        {
            try
            {
                var arrRecords = new string[] {};

                if (inputBuffer != string.Empty)
                    arrRecords = inputBuffer.Split(new[] {_clsRRecord.Rules.EndOfRecordCharacter},
                        StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                bool _newResult = false;
                while (i < arrRecords.Length)
                {
                    string[] arrFields = arrRecords[i].Split(_clsPRecord.Rules.FieldDelimiter);

                    if (arrFields[0].Equals(_clsRRecord.RecordType.Data))
                    {
                        _clsRRecord = new CobasE411ResultRecord(arrRecords[i]);
                        AddResult(_clsRRecord.GetResult());
                        _newResult = true;
                    }
                    if (arrFields[0].StartsWith(_clsPRecord.RecordType.Data))
                        _clsPRecord = new CobasE411PatientInformationRecord(arrRecords[i]);
                    else if (arrFields[0].StartsWith(_clsORecord.RecordType.Data))
                    {
                        _clsORecord = new CobasE411TestOrderRecord(arrRecords[i]);
                        TestResult.Barcode = _clsORecord.SpecimenId.Data.Trim();
                        string tempDate = _clsORecord.DateTimeResultReportedOrLastModified.Data;
                        //TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate.Substring(6, 2),
                        //    tempDate.Substring(4, 2), tempDate.Substring(0, 4));
                    }
                    else if (arrFields[0].Equals(_clsQRecord.RecordType.Data))
                    {
                        _clsQRecord = new CobasE411RequestInformationRecord(arrRecords[i]);
                        _sQBarcode = _clsQRecord.GetOrderBarcode();

                        List<string> regList = GetRegList(_sQBarcode);
                        if (regList != null)
                        {
                            if (regList.Count > 0)
                            {
                                Log.Debug(string.Format("So order: {0}", regList.Count));
                                foreach (string s in regList)
                                {
                                    Log.Debug(string.Format("{0}\r\n", s));
                                }
                            }
                            else
                            {
                                Log.Debug("No order!");
                            }
                        }
                        else
                        {
                            Log.Debug("No order!");
                        }
                        orderList = CreateOrderFrame(regList);
                        return true;
                    }
                    else if (arrFields[0].Equals(_clsHRecord.RecordType.Data))
                        _clsHRecord = new CobasE411HeaderRecord(arrRecords[i]);

                    i++;
                }


                if (_newResult)
                {
                    Log.Debug("Begin Import Result");
                    Log.Debug(ImportResults() ? "Import Result Success" : "Error While Import Result");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Fatal Error:{0}", ex.ToString());
            }
            return false;
        }

        private List<string> CreateOrderFrame(List<string> orderList)
        {
            var retList = new List<string>();

            //Tạo Header mới
            var newHeaderRecord = (CobasE411HeaderRecord) _clsHRecord.Clone();
            newHeaderRecord.CommentOrSpecialInstructions.Data = "TSDWN^REPLY";
            newHeaderRecord.SenderNameOrId.Data = ""; // _clsHRecord.ReveiverId.Data;
            newHeaderRecord.ReveiverId.Data = ""; // _clsHRecord.SenderNameOrId.Data;

            //var sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", newHeaderRecord.Create(),DeviceHelper.ETB);
            //var checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //retList.Add(string.Format("{0}{1}{2}",sTemp,checksum,DeviceHelper.CRLF));

            ////Add Patient Record
            //_clsPRecord.SequenceNumber.Data = "1";
            //_clsPRecord.PatientSex.Data = "U";
            ////_clsPRecord.LaboratoryAssignedPatientId.Data = _sQBarcode;

            //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", _clsPRecord.Create(), DeviceHelper.ETB);

            //checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

            string sTemp = newHeaderRecord.Create();
            _clsPRecord.SequenceNumber.Data = "1";
            sTemp = string.Concat(sTemp, _clsPRecord.Create());
            //_clsPRecord.PatientSex.Data = "U";
            //_clsPRecord.LaboratoryAssignedPatientId.Data = _sQBarcode;
            //Xử lý kết quả )
            if ((orderList != null) && (orderList.Count != 0))
            {
                //Add OrderRecord
                _clsORecord = new CobasE411TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.SpecimenId.Data = _sQBarcode;
                string[] tempIsId = _clsQRecord.StartingRangeIdNumber.Data.Split(_clsQRecord.Rules.ComponentDelimiter);
                _clsORecord.InstrumentSpecimenId.Data = string.Join(_clsORecord.Rules.ComponentDelimiter.ToString(),
                    new[]
                    {
                        tempIsId[3], tempIsId[4], tempIsId[5],
                        tempIsId[6], tempIsId[7], tempIsId[8]
                    });
                _clsORecord.UniversalTestId.Data = _clsORecord.CreateUniversalTestid(orderList);
                _clsORecord.Priority.Data = "R";
                //_clsORecord.SpecimenCollectionDateAndTime.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                //_clsORecord.DateTimeResultReportedOrLastModified.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                _clsORecord.ActionCode.Data = "A";
                _clsORecord.SpecimenDescriptor.Data = "1";
                _clsORecord.ReportTypes.Data = "O";

                //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "3", _clsORecord.Create(), DeviceHelper.ETB);
                sTemp = string.Concat(sTemp, _clsORecord.Create());

                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

                _clsTRecord = new CobasE411TerminationRecord();
                //sTemp = String.Concat(DeviceHelper.STX, "4", _clsTRecord.Create(),DeviceHelper.ETX);
                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

                sTemp = string.Concat(sTemp, _clsTRecord.Create());
            }
            else
            {
                _clsORecord = new CobasE411TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.SpecimenId.Data = _sQBarcode;
                string[] tempIsId = _clsQRecord.StartingRangeIdNumber.Data.Split(_clsQRecord.Rules.ComponentDelimiter);
                _clsORecord.InstrumentSpecimenId.Data = string.Join(_clsORecord.Rules.ComponentDelimiter.ToString(),
                    new[]
                    {
                        tempIsId[3], tempIsId[4], tempIsId[5],
                        tempIsId[6], tempIsId[7], tempIsId[8]
                    });
                _clsORecord.UniversalTestId.Data = string.Empty;
                _clsORecord.Priority.Data = "R";
                _clsORecord.ActionCode.Data = "A";
                _clsORecord.SpecimenDescriptor.Data = "1";
                _clsORecord.ReportTypes.Data = "O";
                sTemp = string.Concat(sTemp, _clsORecord.Create());
                //sTemp = String.Concat(DeviceHelper.STX, "3", _clsORecord.Create(), DeviceHelper.ETB);
                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
                _clsTRecord = new CobasE411TerminationRecord {TerminationCode = {Data = "N"}};
                sTemp = string.Concat(sTemp, _clsTRecord.Create());
                //sTemp = String.Concat(DeviceHelper.STX, "4", _clsTRecord.Create(),DeviceHelper.ETX);
                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
            }
            if (sTemp.Length > 240)
            {
                string sTempFirst = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp.Substring(0, 240),
                    DeviceHelper.ETB);
                string checksum = DeviceHelper.GetCheckSumValue(sTempFirst);
                retList.Add(string.Format("{0}{1}{2}", sTempFirst, checksum, DeviceHelper.CRLF));
                string sTempSecond = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", sTemp.Substring(240),
                    DeviceHelper.ETX);
                checksum = DeviceHelper.GetCheckSumValue(sTempSecond);
                retList.Add(string.Format("{0}{1}{2}", sTempSecond, checksum, DeviceHelper.CRLF));
            }
            else
            {
                string sTempFirst = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp, DeviceHelper.ETX);
                string checksum = DeviceHelper.GetCheckSumValue(sTempFirst);
                retList.Add(string.Format("{0}{1}{2}", sTempFirst, checksum, DeviceHelper.CRLF));
            }

            return retList;
        }
    }
}