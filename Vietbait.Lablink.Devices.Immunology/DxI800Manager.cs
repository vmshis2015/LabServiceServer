using System;
using System.Collections.Generic;
using Vietbait.Lablink.Utilities;
using Vietbait.Lablink.Workflow;

namespace Vietbait.Lablink.Devices.Immunology
{
    public class DxI800Manager : ASTMManager
    {
        private DxI800HeaderRecord _clsHRecord;
        private DxI800TestOrderRecord _clsORecord;
        private DxI800PatientInformationRecord _clsPRecord;
        private DxI800RequestInformationRecord _clsQRecord;
        private DxI800ResultRecord _clsRRecord;
        private DxI800TerminationRecord _clsTRecord;
        private string _sQBarcode;


        public DxI800Manager()
        {
            try
            {
                //if (DateTime.Now > new DateTime(2013, 06, 06)) throw new Exception("XXX");
                _clsHRecord = new DxI800HeaderRecord();
                _clsPRecord = new DxI800PatientInformationRecord();
                _clsORecord = new DxI800TestOrderRecord();
                _clsQRecord = new DxI800RequestInformationRecord();
                _clsRRecord = new DxI800ResultRecord();
                _clsTRecord = new DxI800TerminationRecord();
            }
            catch (Exception ex)
            {
                Log.Error("Fatal Error: {0}", ex);
            }
        }

        ~DxI800Manager()
        {
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
                        _clsRRecord = new DxI800ResultRecord(arrRecords[i]);
                        AddResult(_clsRRecord.GetResult());
                        _newResult = true;
                    }
                    if (arrFields[0].Equals(_clsHRecord.RecordType.Data))
                    {
                        _clsHRecord = new DxI800HeaderRecord(arrRecords[i]);
                        string tempDate = _clsHRecord.DateTime.Data;
                        TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate.Substring(6, 2),
                            tempDate.Substring(4, 2), tempDate.Substring(0, 4));
                    }
                    if (arrFields[0].StartsWith(_clsPRecord.RecordType.Data))
                    {
                        _clsPRecord = new DxI800PatientInformationRecord(arrRecords[i]);
                        
                    }
                    else if (arrFields[0].StartsWith(_clsORecord.RecordType.Data))
                    {
                        _clsORecord = new DxI800TestOrderRecord(arrRecords[i]);
                        TestResult.Barcode = _clsORecord.SpecimenId.Data.Trim();
                    }
                    else if (arrFields[0].Equals(_clsQRecord.RecordType.Data))
                    {
                        _clsQRecord = new DxI800RequestInformationRecord(arrRecords[i]);
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

        private List<string> CreateEachRecord(List<AstmBaseRecord> records,char endCharacter)
        {
            var retList = new List<string>();
            var frameNo = 0;
            foreach (AstmBaseRecord eachRecord in records)
            {
                var sTemp = string.Empty;
                var checksum = string.Empty;
                if (eachRecord.Create().Length > 240)
                {
                    frameNo++;
                    sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, frameNo, eachRecord.Create().Substring(0, 240),
                                          DeviceHelper.ETB);
                    checksum = DeviceHelper.GetCheckSumValue(sTemp);
                    retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
                    frameNo++;
                    sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, frameNo, eachRecord.Create().Substring(240),
                                          endCharacter);
                    checksum = DeviceHelper.GetCheckSumValue(sTemp);
                    retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
                }
                else
                {
                    frameNo++;
                    sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, frameNo, eachRecord.Create(),
                                          endCharacter);
                    checksum = DeviceHelper.GetCheckSumValue(sTemp);
                    retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
                }
            }
            return retList;
        } 

        private List<string> CreateOrderFrame(List<string> orderList)
        {
            var astmBaseRecordsList = new List<AstmBaseRecord>();
            var retList = new List<string>();
            //Tạo Header mới
            var newHeaderRecord = (DxI800HeaderRecord) _clsHRecord.Clone();
            //newHeaderRecord.CommentOrSpecialInstructions.Data = "TSDWN^REPLY";
            newHeaderRecord.SenderNameOrId.Data = ""; // _clsHRecord.ReveiverId.Data;
            newHeaderRecord.ReceiverId.Data = ""; // _clsHRecord.SenderNameOrId.Data;
           // string sTemp = newHeaderRecord.Create();
            astmBaseRecordsList.Add(newHeaderRecord);
            //var sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", newHeaderRecord.Create(), DeviceHelper.ETX);
            //var checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

            ////Add Patient Record
            //_clsPRecord.SequenceNumber.Data = "1";
            //_clsPRecord.PatientSex.Data = "U";
            ////_clsPRecord.LaboratoryAssignedPatientId.Data = _sQBarcode;

            //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", _clsPRecord.Create(), DeviceHelper.ETB);

            //checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

            
            
            //Xử lý kết quả nếu có chỉ định thì xử lý chuỗi P và O
            //Nếu không có thì không hiển thị chuỗi P và O
            if ((orderList != null) && (orderList.Count != 0))
            {
                _clsPRecord.SequenceNumber.Data = "1";
                _clsPRecord.PracticeAssignedPatientID.Data = string.Format("{0}{1}",DateTime.Now.ToString("yyMMdd"),_sQBarcode);
                //sTemp = string.Concat(sTemp, _clsPRecord.Create());

                //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", _clsPRecord.Create(), DeviceHelper.ETX);
                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
                astmBaseRecordsList.Add(_clsPRecord);
                //Add OrderRecord
                _clsORecord = new DxI800TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.SpecimenId.Data = _sQBarcode;
                //string[] tempIsId = _clsQRecord.StartingRangeIdNumber.Data.Split(_clsQRecord.Rules.ComponentDelimiter);
                //_clsORecord.InstrumentSpecimenId.Data = string.Join(_clsORecord.Rules.ComponentDelimiter.ToString(),
                //    new[]
                //    {
                //        tempIsId[3], tempIsId[4], tempIsId[5],
                //        tempIsId[6], tempIsId[7], tempIsId[8]
                //    });
                _clsORecord.UniversalTestId.Data = _clsORecord.CreateUniversalTestid(orderList);
                _clsORecord.Priority.Data = "R";
                //_clsORecord.SpecimenCollectionDateAndTime.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                //_clsORecord.DateTimeResultReportedOrLastModified.Data = DateTime.Now.ToString("yyyyMMddHHmmss");
                _clsORecord.ActionCode.Data = "A";
                _clsORecord.SpecimenDescriptor.Data = "Serum";

                //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "3", _clsORecord.Create(), DeviceHelper.ETX);
                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
                //_clsORecord.ReportTypes.Data = "F";
                astmBaseRecordsList.Add(_clsORecord);
                //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "3", _clsORecord.Create(), DeviceHelper.ETB);
                //sTemp = string.Concat(sTemp, _clsORecord.Create());

                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));


                //sTemp = String.Concat(DeviceHelper.STX, "4", _clsTRecord.Create(),DeviceHelper.ETX);
                //checksum = DeviceHelper.GetCheckSumValue(sTemp);
                //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
            }

            _clsTRecord = new DxI800TerminationRecord { TerminationCode = { Data = "F" } };
            //sTemp = string.Concat(sTemp, _clsTRecord.Create());
            astmBaseRecordsList.Add(_clsTRecord);
            //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", _clsTRecord.Create(), DeviceHelper.ETX);
            //checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));

            //else
            //{
            //    _clsORecord = new DxI800TestOrderRecord();
            //    _clsORecord.SequenceNumber.Data = "1";
            //    _clsORecord.SpecimenId.Data = _sQBarcode;
            //    //string[] tempIsId = _clsQRecord.StartingRangeIdNumber.Data.Split(_clsQRecord.Rules.ComponentDelimiter);
            //    //_clsORecord.InstrumentSpecimenId.Data = string.Join(_clsORecord.Rules.ComponentDelimiter.ToString(),
            //    //    new[]
            //    //    {
            //    //        tempIsId[3], tempIsId[4], tempIsId[5],
            //    //        tempIsId[6], tempIsId[7], tempIsId[8]
            //    //    });
            //    _clsORecord.UniversalTestId.Data = string.Empty;
            //    _clsORecord.Priority.Data = "R";
            //    _clsORecord.ActionCode.Data = "A";
            //    _clsORecord.SpecimenDescriptor.Data = "1";
            //    //_clsORecord.ReportTypes.Data = "O";
            //    sTemp = string.Concat(sTemp, _clsORecord.Create());
            //    //sTemp = String.Concat(DeviceHelper.STX, "3", _clsORecord.Create(), DeviceHelper.ETB);
            //    //checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //    //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
            //    _clsTRecord = new DxI800TerminationRecord { TerminationCode = { Data = "F" } };
            //    sTemp = string.Concat(sTemp, _clsTRecord.Create());
            //    //sTemp = String.Concat(DeviceHelper.STX, "4", _clsTRecord.Create(),DeviceHelper.ETX);
            //    //checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //    //retList.Add(string.Format("{0}{1}{2}", sTemp, checksum, DeviceHelper.CRLF));
            //}
            
            //if (sTemp.Length > 240)
            //{
            //    string sTempFirst = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp.Substring(0, 240),
            //        DeviceHelper.ETB);
            //    string checksum = DeviceHelper.GetCheckSumValue(sTempFirst);
            //    retList.Add(string.Format("{0}{1}{2}", sTempFirst, checksum, DeviceHelper.CRLF));
            //    string sTempSecond = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "2", sTemp.Substring(240),
            //        DeviceHelper.ETX);
            //    checksum = DeviceHelper.GetCheckSumValue(sTempSecond);
            //    retList.Add(string.Format("{0}{1}{2}", sTempSecond, checksum, DeviceHelper.CRLF));
            //}
            //else
            //{
            //    string sTempFirst = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp, DeviceHelper.ETX);
            //    string checksum = DeviceHelper.GetCheckSumValue(sTempFirst);
            //    retList.Add(string.Format("{0}{1}{2}", sTempFirst, checksum, DeviceHelper.CRLF));
            //}

            //sTemp = String.Concat(DeviceHelper.STX,"1", sTemp, DeviceHelper.ETX);
            //checksum = DeviceHelper.GetCheckSumValue(sTemp);
            //Log.Debug("Checksum:"+checksum);
            //string orderFrame = String.Concat(sTemp, checksum, DeviceHelper.CRLF);
            //Log.Debug("Order frame1: <"+ orderFrame);
            retList = CreateEachRecord(astmBaseRecordsList, DeviceHelper.ETX);
            return retList;
        }
    }
}