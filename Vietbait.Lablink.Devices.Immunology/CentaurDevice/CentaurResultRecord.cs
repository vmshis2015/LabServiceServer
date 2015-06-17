using System;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology.CentaurDevice
{
    public class CentaurResultRecord : ResultRecord
    {
        #region attributries

        private FieldOfRecord _prvDataValue;
        private FieldOfRecord _prvDateTestCompleted;
        private FieldOfRecord _prvSeqNumber;
        private FieldOfRecord _prvTestIdString;
        private FieldOfRecord _prvUnits;

        #endregion

        public CentaurResultRecord(string[] sInput) : this()
        {
            PrvParseData(sInput);
        }

        public CentaurResultRecord()
        {
            RecordType = "R";
            RecordDelimiter = DeviceHelper.CR;
            FieldDelimiter = '|';
            RepeatDelimiter = '\\';
            ComponentDelimiter = '^';
            EscDelimiter = '&';
            _prvSeqNumber.PubFieldData = "";
            _prvSeqNumber.PubFieldIndex = 2;
            _prvTestIdString.PubFieldData = "";
            _prvTestIdString.PubFieldIndex = 3;
            _prvDataValue.PubFieldData = "";
            _prvDataValue.PubFieldIndex = 4;
            _prvUnits.PubFieldData = "";
            _prvUnits.PubFieldIndex = 5;
            _prvDateTestCompleted.PubFieldData = "";
            _prvDateTestCompleted.PubFieldIndex = 13;
        }

        public override FieldOfRecord SegNumber
        {
            get { return _prvSeqNumber; }
            set { _prvSeqNumber = value; }
        }

        public override FieldOfRecord TestIdString
        {
            get { return _prvTestIdString; }
            set { _prvTestIdString = value; }
        }

        public override string TestId { get; set; }
        public string ResultAspects { get; set; }

        public override FieldOfRecord DataValue
        {
            get { return _prvDataValue; }
            set { _prvDataValue = value; }
        }

        public string TestDate { get; set; }

        public override sealed char FieldDelimiter { get; set; }

        public override sealed char RepeatDelimiter { get; set; }

        public override sealed char ComponentDelimiter { get; set; }

        public override sealed char EscDelimiter { get; set; }

        public override sealed string RecordType { get; set; }

        public override sealed char RecordDelimiter { get; set; }

        ~CentaurResultRecord()
        {
        }

        private bool PrvParseData(string[] sInput)
        {
            try
            {
                if (sInput[0] != RecordType)
                {
                    return false;
                }
                _prvSeqNumber.PubFieldData = sInput[_prvSeqNumber.PubFieldIndex - 1];
                _prvTestIdString.PubFieldData = sInput[_prvTestIdString.PubFieldIndex - 1];
                _prvDataValue.PubFieldData = sInput[_prvDataValue.PubFieldIndex - 1];
                string[] sTemp = _prvTestIdString.PubFieldData.Split(ComponentDelimiter);
                TestId = sTemp[3];
                ResultAspects = sTemp[7];
                _prvUnits.PubFieldData = sInput[_prvUnits.PubFieldIndex - 1];
                _prvDateTestCompleted.PubFieldData = sInput[_prvDateTestCompleted.PubFieldIndex - 1];
                TestDate = _prvDateTestCompleted.PubFieldData.Substring(0, 8);
                TestDate = string.Format("{0}/{1}/{2}", TestDate.Substring(6, 2), TestDate.Substring(4, 2),
                    TestDate.Substring(0, 4));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool ParseData(string[] sInput)
        {
            throw new NotImplementedException();
        }

        public override string CreateData()
        {
            throw new NotImplementedException();
        }
    };
}