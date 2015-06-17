using System;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology
{
    public class VersaCellQueryRecord : QueryRecord
    {
        #region attributries

        private FieldOfRecord _prvEndingRangeId;
        private FieldOfRecord _prvRequestInforStatusCode;
        private FieldOfRecord _prvSeqNumber;
        private FieldOfRecord _prvStartingRangeId;
        private FieldOfRecord _prvTestIdString;

        #endregion

        public VersaCellQueryRecord(string[] sInput) : this()
        {
            ParseData(sInput);
        }

        public VersaCellQueryRecord()
        {
            RecordType = "Q";
            RecordDelimiter = DeviceHelper.CR;
            FieldDelimiter = '|';
            RepeatDelimiter = '\\';
            ComponentDelimiter = '^';
            EscDelimiter = '&';
            _prvSeqNumber.PubFieldData = "1";
            _prvSeqNumber.PubFieldIndex = 2;
            _prvStartingRangeId.PubFieldData = "";
            _prvStartingRangeId.PubFieldIndex = 3;
            _prvEndingRangeId.PubFieldData = "";
            _prvEndingRangeId.PubFieldIndex = 4;
            _prvTestIdString.PubFieldData = "";
            _prvTestIdString.PubFieldIndex = 5;
            _prvRequestInforStatusCode.PubFieldData = "";
            _prvRequestInforStatusCode.PubFieldIndex = 13;
            QueryBarcode = string.Empty;
        }

        public override sealed FieldOfRecord SeqNumber
        {
            get { return _prvSeqNumber; }
            set { _prvSeqNumber = value; }
        }

        public override sealed string QueryBarcode { get; set; }

        public override sealed FieldOfRecord StartingRangeId
        {
            get { return _prvStartingRangeId; }
            set { _prvStartingRangeId = value; }
        }

        public override sealed FieldOfRecord EndingRangeId
        {
            get { return _prvEndingRangeId; }
            set { _prvEndingRangeId = value; }
        }

        public override sealed FieldOfRecord TestIdString
        {
            get { return _prvTestIdString; }
            set { _prvTestIdString = value; }
        }

        public override sealed FieldOfRecord RequestInforStatusCode
        {
            get { return _prvRequestInforStatusCode; }
            set { _prvRequestInforStatusCode = value; }
        }

        public override sealed char FieldDelimiter { get; set; }

        public override sealed char RepeatDelimiter { get; set; }

        public override sealed char ComponentDelimiter { get; set; }

        public override sealed char EscDelimiter { get; set; }

        public override sealed string RecordType { get; set; }

        public override sealed char RecordDelimiter { get; set; }

        public override bool ParseData(string[] sInput)
        {
            try
            {
                if (sInput[0] != RecordType)
                {
                    return false;
                }

                _prvSeqNumber.PubFieldData = sInput[_prvSeqNumber.PubFieldIndex - 1];
                _prvStartingRangeId.PubFieldData = sInput[_prvStartingRangeId.PubFieldIndex - 1];
                if (!string.IsNullOrEmpty(_prvStartingRangeId.PubFieldData))
                {
                    string[] temp = _prvStartingRangeId.PubFieldData.Split(ComponentDelimiter);
                    if (temp.Length > 1) QueryBarcode = temp[1];
                }
                _prvEndingRangeId.PubFieldData = sInput[_prvEndingRangeId.PubFieldIndex - 1];
                _prvTestIdString.PubFieldData = sInput[_prvTestIdString.PubFieldIndex - 1];
                _prvRequestInforStatusCode.PubFieldData = sInput[_prvRequestInforStatusCode.PubFieldIndex - 1];

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public override string CreateData()
        {
            var sTmpArray = new string[26];
            sTmpArray[_prvSeqNumber.PubFieldIndex - 1] = _prvSeqNumber.PubFieldData;
            sTmpArray[_prvStartingRangeId.PubFieldIndex - 1] = _prvStartingRangeId.PubFieldData;
            sTmpArray[_prvEndingRangeId.PubFieldIndex - 1] = _prvEndingRangeId.PubFieldData;
            sTmpArray[_prvTestIdString.PubFieldIndex - 1] = _prvTestIdString.PubFieldData;
            sTmpArray[_prvRequestInforStatusCode.PubFieldIndex - 1] = _prvRequestInforStatusCode.PubFieldData;
            string sTmp = string.Join(FieldDelimiter.ToString(), sTmpArray);
            return sTmp;
        }
    }
}