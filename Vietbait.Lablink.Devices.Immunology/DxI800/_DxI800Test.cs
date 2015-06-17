using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology
{
    [TestFixture]
    internal class _DxI800Test
    {
        [Test]
        public void HeaderTest()
        {
            try
            {
                const string rawHeader = @"1H|\^&|||ACCESS^600138|||||LIS||P|1|20130507152052";
                var oldHeader = new DxI800HeaderRecord();
                oldHeader.Parse(rawHeader);
                //h.Parse(rawHeader);
                Debug.WriteLine(oldHeader.RecordType);
                var newHeader = (DxI800HeaderRecord) oldHeader.Clone();
                newHeader.ReceiverId.Data = oldHeader.ReceiverId.Data;
                newHeader.RecordType.Data = oldHeader.RecordType.Data.PadRight(1);
                newHeader.SenderNameOrId.Data = oldHeader.SenderNameOrId.Data;
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void OrderTest()
        {
            try
            {
                const string rawHeader = @"3O|1|101|^2^1|^^^FRT4^1|||||||||||Serum||||||||||F";
                var order = new DxI800TestOrderRecord(rawHeader);

                var _clsORecord = new DxI800TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.SpecimenId.Data = "AABB1234";
                List<string> orderList = new List<string> { "Ferritin", "Theo" };
                _clsORecord.UniversalTestId.Data = _clsORecord.CreateUniversalTestid(orderList);
                _clsORecord.Priority.Data = "R";

                _clsORecord.ActionCode.Data = "A";
                _clsORecord.SpecimenDescriptor.Data = "Serum";

                //sTemp = string.Concat(sTemp, _clsORecord.Create());

                Debug.WriteLine(_clsORecord.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void PatientInforTest()
        {
            try
            {
                var newHeader = new DxI800PatientInformationRecord(@"P|1|CasperJane|||Johnson^Joan||19580101|F");
                var tempP = new DxI800PatientInformationRecord(@"P|1|AbelCindy");
                Debug.WriteLine(tempP.Create());
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void QueryTest()
        {
            try
            {
                var newHeader = new DxI800RequestInformationRecord(@"2Q|1|^Samp45||ALL||||||||O");
                //var tempP = new DxI800PatientInformationRecord(@"P|1|||||||M||||||35^Y||||||||||||||||||||");
                //Debug.WriteLine(tempP.Create());
                Debug.WriteLine(newHeader.GetOrderBarcode());
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void ResultTest()
        {
            try
            {
                //var newHeader = new DxI800ResultRecord(@"R|1|^^^2/1/not|8.60|nmol/L||N||F||BMSERV|||E11");
                var newHeader =
                    new DxI800ResultRecord(@"R|1|^^^TSH^1|0.18|uIU/mL||N||F||||20001010113536");

                //Debug.WriteLine(tempP.Create());
                Debug.WriteLine(newHeader.Create());
                ResultItem resultItem = newHeader.GetResult();
                Debug.WriteLine(resultItem.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void TerminationTest()
        {
            try
            {
                var newHeader = new DxI800TerminationRecord();
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void Test()
        {
            try
            {
                string a = DeviceHelper.GetCheckSumValue("a");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private List<string> CreateEachRecord(List<AstmBaseRecord> records, char endCharacter)
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
                    sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, frameNo, eachRecord.Create().Substring(0,240),
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

        [Test]
        public void MessagesTest()
        {
            try
            {
                var astmBaseRecordsList = new List<AstmBaseRecord>();
                List<string> retList = new List<string>();
                var _clsHRecord = new DxI800HeaderRecord(@"H|\^&||||||||||P|1|20130507152052");
                astmBaseRecordsList.Add(_clsHRecord);

                var _clsPRecord = new DxI800PatientInformationRecord(@"P|1|CasperJane|||Johnson^Joan||19580101|F");
                astmBaseRecordsList.Add(_clsPRecord);

                var _clsORecord = new DxI800TestOrderRecord();
                _clsORecord.SequenceNumber.Data = "1";
                _clsORecord.SpecimenId.Data = "3120";
                List<string> orderList = new List<string> { "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin", "Ferritin" };
                _clsORecord.UniversalTestId.Data = _clsORecord.CreateUniversalTestid(orderList);
                _clsORecord.Priority.Data = "R";

                _clsORecord.ActionCode.Data = "A";
                _clsORecord.SpecimenDescriptor.Data = "Serum";
                astmBaseRecordsList.Add(_clsORecord);

                var _clsTRecord = new DxI800TerminationRecord { TerminationCode = { Data = "F" } };
                astmBaseRecordsList.Add(_clsTRecord);
                //sTemp = string.Format("{0}{1}{2}{3}", DeviceHelper.STX, "1", sTemp, DeviceHelper.ETX);

                //var retList = new List<string>();
                var a = CreateEachRecord(astmBaseRecordsList, DeviceHelper.ETX);

                //Debug.WriteLine(sTemp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}