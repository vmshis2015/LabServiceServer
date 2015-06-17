using System;
using System.Collections.Generic;
using Vietbait.Lablink.Devices.Immunology.ImmuliteXpi;
using Vietbait.Lablink.Utilities;
using Vietbait.Lablink.Workflow;

namespace Vietbait.Lablink.Devices.Immunology
{
    internal class ImmuliteXpiManager : ASTMManager
    {
        private ImmuliteXpiHeaderRecord _clsHRecord;
        private ImmuliteXpiTestOrderRecord _clsORecord;
        private ImmuliteXpiPatientInformationRecord _clsPRecord;
        private ImmuliteXpiRequestInformationRecord _clsQRecord;
        private ImmuliteXpiResultRecord _clsRRecord;
        private ImmuliteXpiTerminationRecord _clsTRecord;
        private string _sQBarcode;


        public ImmuliteXpiManager()
        {
            try
            {
                _clsHRecord = new ImmuliteXpiHeaderRecord();
                _clsPRecord = new ImmuliteXpiPatientInformationRecord();
                _clsORecord = new ImmuliteXpiTestOrderRecord();
                _clsQRecord = new ImmuliteXpiRequestInformationRecord();
                _clsRRecord = new ImmuliteXpiResultRecord();
                _clsTRecord = new ImmuliteXpiTerminationRecord();
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

                    if (arrFields[0].StartsWith(_clsPRecord.RecordType.Data))
                    {
                        _clsPRecord = new ImmuliteXpiPatientInformationRecord(arrRecords[i]);
                        if (newResult)
                        {
                            Log.Debug("Begin Import {0} Result", TestResult.Items.Count);
                            Log.Debug(ImportResults() ? "Import Result Success" : "Error While Import Result");
                            newResult = false;
                        }
                    }
                    else if (arrFields[0].StartsWith(_clsORecord.RecordType.Data))
                    {
                        _clsORecord = new ImmuliteXpiTestOrderRecord(arrRecords[i]);
                        string barcode = _clsORecord.SpecimenId.Data.Trim();
                        while (barcode.IndexOf(".") >= 0)
                        {
                            barcode = barcode.Replace(".", "");
                        }
                        TestResult.Barcode = barcode;
                    }
                    else if (arrFields[0].Equals(_clsRRecord.RecordType.Data))
                    {
                        _clsRRecord = new ImmuliteXpiResultRecord(arrRecords[i]);

                        if (string.IsNullOrEmpty(TestResult.TestDate))
                        {
                            string tempDate = _clsRRecord.DateTimeTestCompleted.Data.Trim();
                            TestResult.TestDate = string.IsNullOrEmpty(tempDate)
                                ? DateTime.Now.ToString("dd/MM/yyyy")
                                : string.Format("{0}/{1}/{2}", tempDate.Substring(6, 2),
                                    tempDate.Substring(4, 2), tempDate.Substring(0, 4));
                        }
                        
                        
                        AddResult(_clsRRecord.GetResult());
                        newResult = true;
                    }
                    else if (arrFields[0].Equals(_clsQRecord.RecordType.Data))
                    {
                        string patientName = "";
                        _clsQRecord = new ImmuliteXpiRequestInformationRecord(arrRecords[i]);
                        _sQBarcode = _clsQRecord.GetOrderBarcode();
                        Log.Debug("Barcode is:{0}", _sQBarcode);

                        List<string> regList;

                        string barcodeToQuery = _sQBarcode;
                        if (_sQBarcode.Length <= 4)
                            barcodeToQuery = string.Format("{0}{1}", DateTime.Now.ToString("yyMMdd"),
                                _sQBarcode.PadLeft(4, '0'));
                        regList = GetRegList(barcodeToQuery, ref patientName);

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
                        _clsHRecord = new ImmuliteXpiHeaderRecord(arrRecords[i]);

                    i++;
                }

                if (newResult)
                {
                    Log.Debug("Begin Import {0} Result", TestResult.Items.Count);
                    Log.Debug(ImportResults() ? "Import Result Success" : "Error While Import Result");
                    return true;
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
            var newHeaderRecord = (ImmuliteXpiHeaderRecord) _clsHRecord.Clone();
            newHeaderRecord.SenderNameOrId.Data = _clsHRecord.ReceiverID.Data;
            newHeaderRecord.ReceiverID.Data = _clsHRecord.SenderNameOrId.Data;
            newHeaderRecord.CurrentDate.Data = DateTime.Now.ToString("yyyyMMddHHmmss");

            string sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", newHeaderRecord.Create(),
                DeviceHelper.ETB);
            string checksum = DeviceHelper.GetCheckSumValue(sTemp);
            retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

            //string sTemp = newHeaderRecord.Create();
            _clsPRecord.SequenceNumber.Data = "1";

            //Todo: TestOnly
            //_clsPRecord.PatientId.Data = "0003";
            //_clsPRecord.PatientName.Data = "NGUYEN VAN A^^";
            //_clsPRecord.PatientSex.Data = "U";
            _clsPRecord.PracticeAssignedPatientID.Data = string.Format("{0}{1}", DateTime.Now.ToString("yyMMdd"),
                _sQBarcode);
            _clsPRecord.Birthdate.Data = "20000101";
            _clsPRecord.PatientName.Data = string.Format("{0}^^", patientName);
            _clsPRecord.PatientSex.Data = "M";

            sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", _clsPRecord.Create(), DeviceHelper.ETB);
            checksum = DeviceHelper.GetCheckSumValue(sTemp);
            retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

            //Xử lý kết quả )
            if ((orderList != null) && (orderList.Count != 0))
            {
                //Add OrderRecord
                _clsORecord = new ImmuliteXpiTestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                //_clsORecord.SpecimenId.Data = string.Format("{0}{1}",DateTime.Now.ToString("yyMMdd"),_sQBarcode);
                _clsORecord.SpecimenId.Data = string.Format("{0}",_sQBarcode);
                _clsORecord.RequestedDateAndTime.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                _clsORecord.SpecimenCollectionDateAndTime.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                _clsORecord.UniversalTestId.Data = _clsORecord.CreateUniversalTestid(orderList);
                sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "3", _clsORecord.Create(), DeviceHelper.ETB);
                checksum = DeviceHelper.GetCheckSumValue(sTemp);
                retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

                _clsTRecord = new ImmuliteXpiTerminationRecord {TerminationCode = {Data = "F"}};
                sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "4", _clsTRecord.Create(), DeviceHelper.ETX);
                checksum = DeviceHelper.GetCheckSumValue(sTemp);
                retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
            }
            else
            {
                _clsTRecord = new ImmuliteXpiTerminationRecord {TerminationCode = {Data = "F"}};
                sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "3", _clsTRecord.Create(), DeviceHelper.ETX);
                checksum = DeviceHelper.GetCheckSumValue(sTemp);
                retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
            }
            return retList;
        }
    }
}