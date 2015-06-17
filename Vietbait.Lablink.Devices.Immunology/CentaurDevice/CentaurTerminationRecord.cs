using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology.CentaurDevice

{
    internal class CentaurTerminationRecord : ITerminationRecord
    {
        #region Implementation of ITerminationRecord

        public string TerminationRecord { get; set; }

        public byte SequenceNumber { get; set; }


        public string TerminationCode { get; set; }

        #endregion

        public CentaurTerminationRecord()
        {
            TerminationRecord = string.Concat("L|1|F", DeviceHelper.CR);
        }

        public string CreateData()
        {
            return string.Concat("L|1|", TerminationCode, DeviceHelper.CR);
        }
    }
}