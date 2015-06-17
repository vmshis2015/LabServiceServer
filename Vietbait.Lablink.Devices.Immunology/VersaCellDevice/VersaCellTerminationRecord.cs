using System;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology
{
    internal class VersaCellTerminationRecord : ITerminationRecord, IDeviceRecord
    {
        #region Implementation of ITerminationRecord

        public char FieldDelimiter { get; set; }

        public char RepeatDelimiter { get; set; }

        public char ComponentDelimiter { get; set; }

        public char EscDelimiter { get; set; }

        public string RecordType { get; set; }

        public char RecordDelimiter { get; set; }
        public string TerminationRecord { get; set; }

        public byte SequenceNumber { get; set; }

        public string TerminationCode { get; set; }

        #endregion

        public VersaCellTerminationRecord()
        {
            RecordDelimiter = '|';
            RecordType = @"L";
        }

        public VersaCellTerminationRecord(byte sequenceNumber, string terminationCode) : this()
        {
            SequenceNumber = sequenceNumber;
            TerminationCode = terminationCode;
            TerminationRecord = CreateData(sequenceNumber, terminationCode);
        }


        public bool ParseData(string[] sInput)
        {
            throw new NotImplementedException();
        }

        public string CreateData()
        {
            string[] sTemp = {RecordType, SequenceNumber.ToString(), TerminationCode};
            return string.Concat(string.Join(RecordDelimiter.ToString(), sTemp), DeviceHelper.CR);
        }

        public string CreateData(byte sequenceNumber, string terminationCode)
        {
            string[] sTemp = {RecordType, SequenceNumber.ToString(), TerminationCode};
            return string.Concat(string.Join(RecordDelimiter.ToString(), sTemp), DeviceHelper.CR);
        }
    }
}