using System;
using System.Collections.Generic;
using Vietbait.Lablink.Utilities;
using Vietbait.Lablink.Workflow;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    public class Bs800Manager : TcpIpASTMManager
    {
        private Bs800HeaderRecord _clsHRecord;
        private Bs800TestOrderRecord _clsORecord;
        private Bs800PatientInformationRecord _clsPRecord;
        private Bs800RequestInformationRecord _clsQRecord;
        private Bs800ResultRecord _clsRRecord;
        private Bs800TerminationRecord _clsTRecord;
        private string _sQBarcode;


        public Bs800Manager()
        {
            try
            {
                _clsHRecord = new Bs800HeaderRecord();
                _clsPRecord = new Bs800PatientInformationRecord();
                _clsORecord = new Bs800TestOrderRecord();
                _clsQRecord = new Bs800RequestInformationRecord();
                _clsRRecord = new Bs800ResultRecord();
                _clsTRecord = new Bs800TerminationRecord();
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
                bool newResult = false;
                while (i < arrRecords.Length)
                {
                    string[] arrFields = arrRecords[i].Split(_clsPRecord.Rules.FieldDelimiter);

                    if (arrFields[0].Equals(_clsRRecord.RecordType.Data))
                    {
                        _clsRRecord = new Bs800ResultRecord(arrRecords[i]);
                        AddResult(_clsRRecord.GetResult());
                        newResult = true;
                    }
                    else if (arrFields[0].StartsWith(_clsPRecord.RecordType.Data))
                        _clsPRecord = new Bs800PatientInformationRecord(arrRecords[i]);
                    else if (arrFields[0].StartsWith(_clsORecord.RecordType.Data))
                    {
                        _clsORecord = new Bs800TestOrderRecord(arrRecords[i]);
                        string barcode = _clsORecord.InstrumentSpecimenId.Data.Trim();
                        while (barcode.IndexOf(".") >= 0)
                        {
                            barcode = barcode.Replace(".", "");
                        }
                        TestResult.Barcode = barcode;

                        string tempDate = _clsORecord.SpecimenCollectionDateAndTime.Data.Trim() == ""
                            ? _clsORecord.RequestedDateAndTime.Data.Trim()
                            : _clsORecord.SpecimenCollectionDateAndTime.Data.Trim();

                        TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate.Substring(6, 2),
                            tempDate.Substring(4, 2), tempDate.Substring(0, 4));
                    }
                    else if (arrFields[0].Equals(_clsQRecord.RecordType.Data))
                    {
                        string patientName = "";
                        _clsQRecord = new Bs800RequestInformationRecord(arrRecords[i]);
                        _sQBarcode = _clsQRecord.GetOrderBarcode();
                        Log.Debug("Barcode is:{0}", _sQBarcode);

                        var regList = new List<string>();
                        //if(System.Diagnostics.Debugger.IsAttached)
                        //{
                        //    regList.AddRange(new[]{"002","006"});
                        //}
                        //else
                        //{
                        regList = GetRegList(_sQBarcode.Replace(".", ""), ref patientName);
                        //}

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
                        orderList = CreateOrderFrame(regList, patientName);
                        return true;
                    }
                    else if (arrFields[0].Equals(_clsHRecord.RecordType.Data))
                        _clsHRecord = new Bs800HeaderRecord(arrRecords[i]);

                    i++;
                }

                if (newResult)
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

        private List<string> CreateOrderFrame(List<string> orderList, string patientName)
        {
            var retList = new List<string>();

            //Tạo Header mới
            var newHeaderRecord = (Bs800HeaderRecord) _clsHRecord.Clone();
            newHeaderRecord.SenderNameOrId.Data = "LIS^^"; // _clsHRecord.ReveiverId.Data;
            newHeaderRecord.ProcessingId.Data = "QA";
            newHeaderRecord.CurrentDate.Data = DateTime.Now.ToString("yyyyMMddHHmmss");

            string sTemp = newHeaderRecord.Create();
            _clsPRecord.SequenceNumber.Data = "1";

            //Todo: TestOnly
            //_clsPRecord.PatientId.Data = "0003";
            //_clsPRecord.PatientName.Data = "NGUYEN VAN A^^";
            //_clsPRecord.PatientSex.Data = "U";
            _clsPRecord.PatientId.Data = _sQBarcode;
            _clsPRecord.PatientName.Data = string.Format("{0}^^", patientName);
            _clsPRecord.PatientSex.Data = "U";
            sTemp = string.Concat(sTemp, _clsPRecord.Create());

            //Xử lý kết quả )
            if ((orderList != null) && (orderList.Count != 0))
            {
                //Add OrderRecord
                _clsORecord = new Bs800TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.InstrumentSpecimenId.Data = _sQBarcode;
                _clsORecord.SampleId.Data = _sQBarcode;
                _clsORecord.UniversalTestId.Data = _clsORecord.CreateUniversalTestid(orderList);
                _clsORecord.Priority.Data = "R";
                _clsORecord.ReportTypes.Data = "Q";
                _clsORecord.RequestedDateAndTime.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                _clsORecord.SpecimenCollectionDateAndTime.Data = _clsORecord.RequestedDateAndTime.Data;
                sTemp = string.Concat(sTemp, _clsORecord.Create());
                _clsTRecord = new Bs800TerminationRecord();
                sTemp = string.Concat(sTemp, _clsTRecord.Create());
            }
            else
            {
                _clsORecord = new Bs800TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.InstrumentSpecimenId.Data = _sQBarcode;
                _clsORecord.SampleId.Data = _sQBarcode;
                _clsORecord.UniversalTestId.Data = "";
                _clsORecord.Priority.Data = "R";
                _clsORecord.ReportTypes.Data = "Q";
                _clsORecord.RequestedDateAndTime.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                _clsORecord.SpecimenCollectionDateAndTime.Data = _clsORecord.RequestedDateAndTime.Data;
                sTemp = string.Concat(sTemp, _clsORecord.Create());
                _clsTRecord = new Bs800TerminationRecord {TerminationCode = {Data = "I"}};
                sTemp = string.Concat(sTemp, _clsTRecord.Create());
            }

            //// Bỏ qua đoạn kiểm tra độ dài của chuỗi

            //if (sTemp.Length > 240)
            //{
            //    string sTempFirst = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp.Substring(0, 240),
            //                                      DeviceHelper.ETB);
            //    string checksum = DeviceHelper.GetCheckSumValue(sTempFirst);
            //    retList.Add(string.Format("{0}{1}{2}", sTempFirst, checksum, DeviceHelper.CRLF));
            //    string sTempSecond = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", sTemp.Substring(240),
            //                                       DeviceHelper.ETX);
            //    checksum = DeviceHelper.GetCheckSumValue(sTempSecond);
            //    retList.Add(string.Format("{0}{1}{2}", sTempSecond, checksum, DeviceHelper.CRLF));
            //}
            //else
            //{
            string sTempFirst = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp, DeviceHelper.ETX);
            string checksum = DeviceHelper.GetCheckSumValue(sTempFirst);
            retList.Add(string.Format("{0}{1}{2}", sTempFirst, checksum, DeviceHelper.CRLF));
            //}

            return retList;
        }
    }
}