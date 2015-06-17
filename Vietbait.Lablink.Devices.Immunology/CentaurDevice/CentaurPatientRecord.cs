using System;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology.CentaurDevice
{
    public class CentaurPatientRecord : PatientRecord
    {
        #region attributries

        private FieldOfRecord _prvPatientName;
        private FieldOfRecord _prvPraticePid;
        private FieldOfRecord _prvSeqNumber;

        #endregion

        public CentaurPatientRecord()
        {
            RecordType = "P";
            RecordDelimiter = DeviceHelper.CR;
            FieldDelimiter = '|';
            RepeatDelimiter = '\\';
            ComponentDelimiter = '^';
            EscDelimiter = '&';
            _prvSeqNumber.PubFieldData = "1";
            _prvSeqNumber.PubFieldIndex = 2;
            _prvPraticePid.PubFieldData = "";
            _prvPraticePid.PubFieldIndex = 3;
            _prvPatientName.PubFieldData = "";
            _prvPatientName.PubFieldIndex = 6;
        }

        public CentaurPatientRecord(string[] sInput)
            : this()
        {
            PrvParseData(sInput);
        }

        public override FieldOfRecord SegNumber
        {
            get { return _prvSeqNumber; }
            set { _prvSeqNumber = value; }
        }

        public override FieldOfRecord PraticePid
        {
            get { return _prvPraticePid; }
            set { _prvPraticePid = value; }
        }

        public override FieldOfRecord PatientName
        {
            get { return _prvPatientName; }
            set { _prvPatientName = value; }
        }

        public override sealed char FieldDelimiter { get; set; }

        public override sealed char RepeatDelimiter { get; set; }

        public override sealed char ComponentDelimiter { get; set; }

        public override sealed char EscDelimiter { get; set; }

        public override sealed string RecordType { get; set; }

        public override char RecordDelimiter { get; set; }

        ~CentaurPatientRecord()
        {
        }

        private bool PrvParseData(string[] sInput)
        {
            if (sInput[0] != RecordType)
            {
                return false;
            }
            _prvSeqNumber.PubFieldData = sInput[_prvSeqNumber.PubFieldIndex - 1];
            _prvPraticePid.PubFieldData = sInput[_prvPraticePid.PubFieldIndex - 1];
            _prvPatientName.PubFieldData = sInput[_prvPatientName.PubFieldIndex - 1];
            return true;
        }

        public override bool ParseData(string[] sInput)
        {
            throw new NotImplementedException();
        }

        public override string CreateData()
        {
            var sTmpArray = new string[26];
            sTmpArray[0] = RecordType;
            sTmpArray[_prvSeqNumber.PubFieldIndex - 1] = _prvSeqNumber.PubFieldData;
            //sTmpArray[_prvPraticePid.PubFieldIndex - 1] = _prvPraticePid.PubFieldData;
            //sTmpArray[_prvPatientName.PubFieldIndex - 1] = _prvPatientName.PubFieldData;
            string sTmp = string.Join(FieldDelimiter.ToString(), sTmpArray);
            return String.Concat(sTmp, RecordDelimiter);
        }
    }
}